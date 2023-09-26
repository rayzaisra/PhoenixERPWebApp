using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IMasterOpeningBalanceService
    {
        Task<List<MasterOpeningBalance>> Get(MasterOpeningBalanceFilter model);
        Task<int> AddEditAsync(MasterOpeningBalance models);
    }

    public class MasterOpeningBalanceService : IMasterOpeningBalanceService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage MasterOpeningBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public MasterOpeningBalanceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddEditAsync(MasterOpeningBalance entity)
        {
            var legalEntityName = await context.LegalEntity.AsNoTracking().Where(x => x.Id == entity.LegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
            var BusinessUnitName = await context.BusinessUnits.AsNoTracking().Where(x => x.Id == entity.BusinessUnitId).Select(x => x.UnitName).FirstOrDefaultAsync();
            var coaIdHistoricalBalance = await context.FinReportControls.AsNoTracking().Where(x => x.ItemCode == ReportControlItemCode.HistoricalBalance).Select(x => x.CoaId).FirstOrDefaultAsync();

            var yearClose = context.FinancialYears.Where(x => int.Parse(x.FinancialYearName) >= int.Parse(entity.YearOb) && x.LegalEntityId == entity.LegalEntityId && x.Status == StatusTransactionName.Close);
            if (yearClose.Count() > 0)
            {
                throw new Exception($"could not do the opening balance process because in {(yearClose.Min(x => int.Parse(x.FinancialYearName))).ToString()} and the legal entity {(legalEntityName ?? "")} was closed");
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (entity.OriginalDebit == null) entity.OriginalDebit = 0;
                    if (entity.OriginalCredit == null) entity.OriginalCredit = 0;

                    entity.Debit = (entity.OriginalDebit ?? 0) * (entity.ExchangeRate ?? 0);
                    entity.Credit = (entity.OriginalCredit ?? 0) * (entity.ExchangeRate ?? 0);

                    var isEdit = context.MasterOpeningBalances.AsNoTracking().Where(x => x.Id == entity.Id).Count() > 0;
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(entity.Id))
                        {
                            entity.Id = Guid.NewGuid().ToString();
                        }

                        if (string.IsNullOrEmpty(entity.MobNumber))
                        {
                            entity.MobNumber = await TransID.GetTransId(context, Code.MasterOpeningBalance, DateTime.Today);
                        }

                        await context.PhoenixAddAsync(entity);
                    }

                    var journal = await context.Journal.AsNoTracking().Where(x => x.ReferenceId == entity.Id).ToListAsync();
                    context.RemoveRange(journal.ToArray());
                    var desc = $"Opening Balance Year : {entity.YearOb}, Legal Entity : {legalEntityName}";
                    if (!string.IsNullOrEmpty(BusinessUnitName)) { desc += $" Business Unit : {BusinessUnitName}"; }
                    var newJournal = new Journal
                    {
                        Id = Guid.NewGuid().ToString(),
                        TransactionDate = Convert.ToDateTime($"{entity.YearOb}/1/1"),
                        ReferenceId = entity.Id,
                        ReferenceNumber = entity.MobNumber,
                        ReferenceDate = Convert.ToDateTime($"{entity.YearOb}/1/1"),
                        Description = desc,
                        DefaultCurrencyId = entity.DefaultCurrencyId,
                        CurrencyId = entity.CurrencyId,
                        ExchangeRate = entity.ExchangeRate,
                        OriginalAmount = entity.OriginalDebit > 0 ? entity.OriginalDebit : entity.OriginalCredit,
                        Debit = entity.Debit,
                        Credit = entity.Credit,
                        AccountId = entity.AccountId,
                        DivisiId = entity.BusinessUnitId,
                        LegalId = entity.LegalEntityId,
                        AffiliateId = entity.AffiliationId,
                        LineCode = "1",
                        TrxSource = SourceJournal.MasterOpeningBalance,
                        IsActive = true
                    };

                    var newJournalHistoricalBalance = new Journal
                    {
                        Id = Guid.NewGuid().ToString(),
                        TransactionDate = Convert.ToDateTime($"{entity.YearOb}/1/1"),
                        ReferenceId = entity.Id,
                        ReferenceNumber = entity.MobNumber,
                        ReferenceDate = Convert.ToDateTime($"{entity.YearOb}/1/1"),
                        Description = desc,
                        DefaultCurrencyId = entity.DefaultCurrencyId,
                        CurrencyId = entity.CurrencyId,
                        ExchangeRate = entity.ExchangeRate,
                        OriginalAmount = entity.OriginalDebit > 0 ? entity.OriginalDebit : entity.OriginalCredit,
                        Debit = entity.Credit,
                        Credit = entity.Debit,
                        AccountId = coaIdHistoricalBalance,
                        DivisiId = entity.BusinessUnitId,
                        LegalId = entity.LegalEntityId,
                        AffiliateId = entity.AffiliationId,
                        LineCode = "2",
                        TrxSource = SourceJournal.MasterOpeningBalance,
                        IsActive = true
                    };

                    await context.PhoenixAddAsync(newJournal);
                    await context.PhoenixAddAsync(newJournalHistoricalBalance);

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<MasterOpeningBalance>> Get(MasterOpeningBalanceFilter model)
        {
            if (!string.IsNullOrEmpty(model.FilterLegalEntityId) && string.IsNullOrEmpty(model.FilterYear))
            {
                var legalEntityName = await context.LegalEntity.Where(x => x.Id == model.FilterLegalEntityId).Select(x => x.LegalEntityName).FirstOrDefaultAsync();
                throw new Exception($"cannot open the opening balance process, please input fiscal year for legal entity {(legalEntityName ?? "")} before proceeding with this process.");
            }

            var result = await (from a in context.MasterOpeningBalances
                                join b in context.ChartOfAccounts on a.AccountId equals b.Id
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                where !(a.IsDeleted ?? false)
                                        && b.Level == "5"
                                        && a.LegalEntityId == model.FilterLegalEntityId
                                        && a.BusinessUnitId == model.FilterBusinessUnitId
                                        && a.YearOb == model.FilterYear
                                        && (b.CodeRec.Split(".", StringSplitOptions.None)[0] == "1"
                                            || b.CodeRec.Split(".", StringSplitOptions.None)[0] == "2"
                                            || b.CodeRec.Split(".", StringSplitOptions.None)[0] == "3")
                                        && (string.IsNullOrEmpty(model.FilterCoaId) ? true : a.AccountId == model.FilterCoaId)
                                select new MasterOpeningBalance
                                {
                                    Id = a.Id,
                                    MobNumber = a.MobNumber,
                                    BusinessUnitId = a.BusinessUnitId,
                                    LegalEntityId = a.LegalEntityId,
                                    AffiliationId = a.AffiliationId,
                                    ReffNumber = a.ReffNumber,
                                    YearOb = a.YearOb,
                                    AccountId = a.AccountId,
                                    DefaultCurrencyId = a.DefaultCurrencyId,
                                    CurrencyId = a.CurrencyId,
                                    OriginalDebit = a.OriginalDebit,
                                    OriginalCredit = a.OriginalCredit,
                                    ExchangeRate = a.ExchangeRate,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    AmountDb01 = a.AmountDb01,
                                    AmountCr01 = a.AmountCr01,
                                    AmountDb02 = a.AmountDb02,
                                    AmountCr02 = a.AmountCr02,
                                    AmountDb03 = a.AmountDb03,
                                    AmountCr03 = a.AmountCr03,
                                    AmountDb04 = a.AmountDb04,
                                    AmountCr04 = a.AmountCr04,
                                    AmountDb05 = a.AmountDb05,
                                    AmountCr05 = a.AmountCr05,
                                    AmountDb06 = a.AmountDb06,
                                    AmountCr06 = a.AmountCr06,
                                    AmountDb07 = a.AmountDb07,
                                    AmountCr07 = a.AmountCr07,
                                    AmountDb08 = a.AmountDb08,
                                    AmountCr08 = a.AmountCr08,
                                    AmountDb09 = a.AmountDb09,
                                    AmountCr09 = a.AmountCr09,
                                    AmountDb10 = a.AmountDb10,
                                    AmountCr10 = a.AmountCr10,
                                    AmountDb11 = a.AmountDb11,
                                    AmountCr11 = a.AmountCr11,
                                    AmountDb12 = a.AmountDb12,
                                    AmountCr12 = a.AmountCr12,
                                    AmountDbTotal = a.AmountDbTotal,
                                    AmountCrTotal = a.AmountCrTotal,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    AccountName = $"{b.CodeRec}-{b.Name5}",
                                    CurrencyName = c.CurrencyName,
                                    CodeRec = b.CodeRec
                                }).ToListAsync();

            var affiliationId = await context.BusinessUnits.AsNoTracking().Where(x => x.Id == model.FilterBusinessUnitId).Select(x => x.AffiliationId).FirstOrDefaultAsync();
            var currencyName = await context.Currencies.AsNoTracking().Where(x => x.Id == model.FilterCurrencyId).Select(x => x.CurrencyName).FirstOrDefaultAsync();


            var result2 = await context.ChartOfAccounts
                                .Where(x => !(x.IsDeleted ?? false)
                                    && x.Level == "5"
                                    && (x.CodeRec.Split(".", StringSplitOptions.None)[0] == "1"
                                        || x.CodeRec.Split(".", StringSplitOptions.None)[0] == "2"
                                        || x.CodeRec.Split(".", StringSplitOptions.None)[0] == "3")
                                    && (!result.Any(y => y.AccountId == x.Id))
                                    && (string.IsNullOrEmpty(model.FilterCoaId) ? true : x.Id == model.FilterCoaId)
                                )
                                .Select(x => new MasterOpeningBalance
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BusinessUnitId = model.FilterBusinessUnitId,
                                    LegalEntityId = model.FilterLegalEntityId,
                                    AffiliationId = affiliationId,
                                    //ReffNumber = a.ReffNumber,
                                    YearOb = model.FilterYear,
                                    AccountId = x.Id,
                                    DefaultCurrencyId = model.FilterCurrencyId,
                                    CurrencyId = context.Banks.Where(y => y.CoaId == x.Id && !(y.IsDeleted ?? false)).Select(y => y.CurrencyId).FirstOrDefault() ??
                                                    context.MasterPettyCashs.Where(y => y.ChartAccountId == x.Id && !(y.IsDeleted ?? false)).Select(y => y.CurrencyId).FirstOrDefault() ??
                                                    model.FilterCurrencyId,
                                    OriginalDebit = 0,
                                    OriginalCredit = 0,
                                    ExchangeRate = 1,
                                    Debit = 0,
                                    Credit = 0,
                                    AccountName = $"{x.CodeRec}-{x.Name5}",
                                    CurrencyName = (from a in context.Banks
                                                    join b in context.Currencies on a.CurrencyId equals b.Id
                                                    where a.CoaId == x.Id && !(a.IsDeleted ?? false)
                                                    select b.CurrencyName).FirstOrDefault() ??
                                                    (from a in context.MasterPettyCashs
                                                     join b in context.Currencies on a.CurrencyId equals b.Id
                                                     where a.ChartAccountId == x.Id && !(a.IsDeleted ?? false)
                                                     select b.CurrencyName).FirstOrDefault() ??
                                                    currencyName,
                                    CodeRec = x.CodeRec
                                }).ToListAsync();
            result.AddRange(result2);
            return result;
        }
    }
}