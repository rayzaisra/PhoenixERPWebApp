using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface IAccountReceivableAgingService
    {
        Task<List<VwAgingAr>> Get(VwAgingArFilter Filter);
        Task<JournalWriteOffAr> Get(string reffNo);
        Task<List<Journal>> GetJournal(string Id);
        Task<int> AddAsync(JournalWriteOffAr entity);
        Task<int> EditAsync(JournalWriteOffAr entity);
        Task<int> CreateJournal(JournalWriteOffAr entity);
        Task<int> EditJournal(JournalWriteOffAr entity);
        Task<int> DeleteSoftAsync(string Id);
    }
    public class AccountReceivableAgingService : IAccountReceivableAgingService
    {
        readonly DataContext context;
        public AccountReceivableAgingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<VwAgingAr>> Get(VwAgingArFilter Filter)
        {
            var result = await context.VwAgingAr
                        .Where(x => (string.IsNullOrEmpty(Filter.ClientId) ? true : x.ClientId == Filter.ClientId) &&
                            (string.IsNullOrEmpty(Filter.LegalEntityId) ? true : x.LegalEntityId == Filter.LegalEntityId) &&
                            (string.IsNullOrEmpty(Filter.MainserviceCategoryId) ? true : x.MainserviceCategoryId.Contains(Filter.MainserviceCategoryId)) &&
                            (string.IsNullOrEmpty(Filter.JobId) ? true : x.JobId.Contains(Filter.JobId)) &&
                            (string.IsNullOrEmpty(Filter.TypeOfExpenseId) ? true : x.TypeOfExpenseId.Contains(Filter.TypeOfExpenseId)) &&
                            (string.IsNullOrEmpty(Filter.CurrencyId) ? true : x.CurrencyId == Filter.CurrencyId) &&
                            (Filter.OriginalAmount == null ? true : x.Amount == Filter.OriginalAmount)
                        ).ToListAsync();
            return result;
        }

        public async Task<JournalWriteOffAr> Get(string reffNo)
        {
            var result = await context.JournalWriteOffAr.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JvReferenceNo == reffNo).FirstOrDefaultAsync();
            if (result == null)
            {
                var ap = await context.Journal.Where(x => x.ReferenceId == reffNo && x.LineCode == "1" && x.TrxSource == SourceJournal.AccountReceivable).FirstOrDefaultAsync() ?? new Journal();
                var woAmount = context.JournalWriteOffAr.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JvReferenceNo == reffNo && x.IsDraft == false).Sum(x => x.Amount);
                result = new JournalWriteOffAr()
                {
                    JobId = ap.JobId,
                    JvDate = DateTime.Now,
                    CurrencyId = ap.CurrencyId,
                    ExchangeRate = ap.ExchangeRate,
                    JvReferenceNo = ap.ReferenceId,
                    LegalEntityId = ap.LegalId,
                    BusinessUnitId = ap.DivisiId,
                    AffiliationId = ap.AffiliateId,
                    Amount = ap.OriginalAmount - woAmount,// (ap.Debit ?? ap.Credit) - woAmount,
                    ReceivableAccount = ap.AccountId,
                    IsDraft = true
                };
            }
            return result;
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = new List<Journal>();
            if (context.JournalWriteOffAr.Where(x => !(x.IsDeleted ?? false) && x.Id == Id && x.IsDraft == false).Count() > 0)
            {
                result = await (from a in context.Journal
                                join b in context.LegalEntity on a.LegalId equals b.Id
                                join e in context.ChartOfAccounts on a.AccountId equals e.Id
                                join f in context.Currencies on a.DefaultCurrencyId equals f.Id
                                where !(a.IsDeleted ?? false) && a.ReferenceId == Id
                                select new Journal
                                {
                                    AccountId = a.AccountId,
                                    AccountName = e.Name5,
                                    ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    LegalEntityName = b.LegalEntityName,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == a.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliateId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    CodeRec = e.CodeRec,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyName = f.CurrencyName,
                                    CurrencySymbol = f.CurrencySymbol,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            }
            else
            {
                result = await (from a in context.JournalTemps
                                join b in context.LegalEntity on a.LegalId equals b.Id
                                join e in context.ChartOfAccounts on a.AccountId equals e.Id
                                join f in context.Currencies on a.DefaultCurrencyId equals f.Id
                                where !(a.IsDeleted ?? false) && a.ReferenceId == Id
                                select new Journal
                                {
                                    AccountId = a.AccountId,
                                    AccountName = e.Name5,
                                    ReferenceNumber = a.ReferenceNumber,
                                    Description = a.Description,
                                    Debit = a.Debit,
                                    Credit = a.Credit,
                                    LegalEntityName = b.LegalEntityName,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == a.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliateId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    CodeRec = e.CodeRec,
                                    CurrencyId = a.CurrencyId,
                                    CurrencyName = f.CurrencyName,
                                    CurrencySymbol = f.CurrencySymbol,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            }
            return result;
        }

        public async Task<int> AddAsync(JournalWriteOffAr entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(JournalWriteOffAr entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournal(JournalWriteOffAr entity)
        {
            return await AddEdit(entity, false, true);
        }

        public async Task<int> EditJournal(JournalWriteOffAr entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(JournalWriteOffAr entity, bool isEdit, bool isJournal)
        {
            //if (isJournal)
            //{
            //    await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.JvDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.JE);
            //}
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!string.IsNullOrEmpty(entity.BusinessUnitId))
                    {
                        entity.AffiliationId = await context.BusinessUnits.Where(x => x.Id == entity.BusinessUnitId).Select(x => x.AffiliationId).FirstOrDefaultAsync();
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.JournalWriteOffAr, DateTime.Today);
                        entity.Code = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today);
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    //Create Journal
                    var defaultCurrencyId = await context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefaultAsync();
                    if (isJournal == true)
                    {
                        if (!string.IsNullOrEmpty(entity.Id))
                        {
                            var journal = context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == entity.Id).ToList();
                            if (journal.Count > 0)
                            {
                                context.JournalTemps.RemoveRange(journal);
                            }
                        }

                        var pcaId = await context.Pcas.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();
                        var pceId = await context.Pces.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();

                        var createJournalWriteOff = new JournalTemp
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = entity.Amount * entity.ExchangeRate,
                            Credit = 0,
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.WriteOffAccount,
                            Referencedate = entity.JvDate,
                            TransactionDate = entity.JvDate,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "1",
                            TrxSource = SourceJournal.JournalWriteOffAr,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId

                        };
                        await context.PhoenixAddAsync(createJournalWriteOff);

                        var createJournalAP = new JournalTemp
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = 0,
                            Credit = entity.Amount * entity.ExchangeRate,
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.ReceivableAccount,
                            Referencedate = entity.JvDate,
                            TransactionDate = entity.JvDate,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "2",
                            TrxSource = SourceJournal.JournalWriteOffAr,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId
                        };
                        await context.PhoenixAddAsync(createJournalAP);
                    }

                    if (!(entity.IsDraft ?? true))
                    {
                        var pcaId = await context.Pcas.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();
                        var pceId = await context.Pces.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();

                        var createJournalWriteOff = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = entity.Amount * entity.ExchangeRate,
                            Credit = 0,
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.WriteOffAccount,
                            ReferenceDate = entity.JvDate,
                            TransactionDate = entity.JvDate,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "1",
                            TrxSource = SourceJournal.JournalWriteOffAr,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId
                        };
                        await context.PhoenixAddAsync(createJournalWriteOff);

                        var createJournalAP = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = 0,
                            Credit = entity.Amount * entity.ExchangeRate,
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.ReceivableAccount,
                            ReferenceDate = entity.JvDate,
                            TransactionDate = entity.JvDate,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "2",
                            TrxSource = SourceJournal.JournalWriteOffAr,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId
                        };
                        await context.PhoenixAddAsync(createJournalAP);
                    }

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

        public async Task<int> DeleteSoftAsync(string Id)
        {
            var get = await context.JournalWriteOffAr.AsNoTracking().Where(x => x.Id == Id).FirstOrDefaultAsync();
            context.PhoenixDelete(get);

            var journalList = await context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == Id).ToListAsync();
            context.PhoenixDeleteRange(journalList.ToArray());
            return await context.SaveChangesAsync();
        }
    }
}
