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
    public interface IAmortizationService : IDataService<Amortization>
    {
        Task<List<Amortization>> GetList(Amortization model);
        Task<List<AmortizationDetail>> GetDetailJournal(string costSharingAllocationId, string condition, decimal amount, string id);
        Task<List<Journal>> GetJournal(string Id);
        Task<int> CreateJournal(Amortization entity);
        Task<int> EditJournal(Amortization entity);
    }

    public class AmortizationService : IAmortizationService
    {
        readonly DataContext context;
        readonly FinancePeriodService financePeriod;

        /// <summary>
        /// And endpoint to manage Amortization
        /// </summary>
        /// <param name="context">Database context</param>
        public AmortizationService(DataContext context, FinancePeriodService financePeriod)
        {
            this.context = context;
            this.financePeriod = financePeriod;
        }

        public async Task<int> AddAsync(Amortization entity)
        {
            return await AddEdit(entity, false, false);
        }

        public Task<int> AddRangeAsync(params Amortization[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Amortization entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Amortization[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Amortization entity)
        {
            context.PhoenixDelete(entity);

            var journalDetailList = await context.AmortizationDetails.AsNoTracking().Where(x => x.AmortizationId == entity.Id).ToListAsync();
            context.PhoenixDeleteRange(journalDetailList.ToArray());

            var journalList = await context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == entity.Id).ToListAsync();
            context.PhoenixDeleteRange(journalList.ToArray());
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Amortization[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Amortization entity)
        {
            return await AddEdit(entity, true, false);
        }

        public Task<int> EditRangeAsync(Amortization entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateJournal(Amortization entity)
        {
            return await AddEdit(entity, false, true);
        }

        public async Task<int> EditJournal(Amortization entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(Amortization entity, bool isEdit, bool isJournal)
        {
            if (isJournal)
            {
                await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.AmotizationDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.AMOR);
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    entity.AffiliationId = await context.BusinessUnits.Where(x => x.Id == entity.BusinessUnitId).Select(x => x.AffiliationId).FirstOrDefaultAsync();

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.Amortization, DateTime.Today);
                        entity.Code = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today);
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.AmortizationDetails.AsNoTracking().Where(x => x.AmortizationId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.AmortizationDetails.RemoveRange(detail);
                        }
                    }

                    if (entity.AmortizationDetails != null)
                    {
                        foreach (var item in entity.AmortizationDetails)
                        {
                            item.AmortizationId = entity.Id;
                            item.AffiliationId = await context.BusinessUnits.Where(x => x.Id == item.BusinessUnitId).Select(x => x.AffiliationId).FirstOrDefaultAsync();
                            await context.PhoenixAddAsync(item);
                        };
                    }

                    //Create Journal
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

                        var createJournalHeader = new JournalTemp
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.AmotizationDescription,
                            Debit = entity.Condition == "Debit" ? entity.Amount * entity.ExchangeRate : 0,
                            Credit = entity.Condition == "Credit" ? entity.Amount * entity.ExchangeRate : 0,
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.CoaId,
                            Referencedate = entity.AmotizationDate,
                            TransactionDate = entity.AmotizationDate,
                            CurrencyId = entity.CurrencyId,
                            //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "1",
                            TrxSource = SourceJournal.Amortization,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                        };

                        await context.PhoenixAddAsync(createJournalHeader);

                        var createJournal = entity.AmortizationDetails
                            .Select(x => new JournalTemp
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = entity.AmotizationDescription,
                                Debit = x.Debit * entity.ExchangeRate,
                                Credit = x.Credit * entity.ExchangeRate,
                                ReferenceId = entity.Id,
                                ReferenceNumber = entity.Code,
                                AccountId = x.CoaId,
                                Referencedate = entity.AmotizationDate,
                                TransactionDate = entity.AmotizationDate,
                                CurrencyId = entity.CurrencyId,
                                //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                ExchangeRate = entity.ExchangeRate,
                                LegalId = entity.LegalEntityId,
                                AffiliateId = x.AffiliationId,
                                DivisiId = x.BusinessUnitId,
                                LineCode = "1",
                                TrxSource = SourceJournal.Amortization,
                                //IsJobClosed = "N",
                                OriginalAmount = (x.Debit ?? 0) != 0 ? x.Debit : x.Credit,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            });

                        await context.PhoenixAddRangeAsync(createJournal.ToArray());
                    }

                    if (!(entity.IsDraft ?? true))
                    {

                        var createJournalHeader = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.AmotizationDescription,
                            Debit = entity.Condition == "Debit" ? entity.Amount * entity.ExchangeRate : 0,
                            Credit = entity.Condition == "Credit" ? entity.Amount * entity.ExchangeRate : 0,
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.CoaId,
                            ReferenceDate = entity.AmotizationDate,
                            TransactionDate = entity.AmotizationDate,
                            CurrencyId = entity.CurrencyId,
                            //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            //AffiliateId = x.AffiliationId,
                            //DivisiId = x.BusinessUnitId,
                            LineCode = "1",
                            TrxSource = SourceJournal.Amortization,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                        };

                        await context.PhoenixAddAsync(createJournalHeader);
                        var createJournal = entity.AmortizationDetails
                            .Select(x => new Journal
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = entity.AmotizationDescription,
                                Debit = x.Debit * entity.ExchangeRate,
                                Credit = x.Credit * entity.ExchangeRate,
                                ReferenceId = entity.Id,
                                ReferenceNumber = entity.Code,
                                AccountId = x.CoaId,
                                ReferenceDate = entity.AmotizationDate,
                                TransactionDate = entity.AmotizationDate,
                                CurrencyId = entity.CurrencyId,
                                //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                ExchangeRate = entity.ExchangeRate,
                                LegalId = entity.LegalEntityId,
                                AffiliateId = x.AffiliationId,
                                DivisiId = x.BusinessUnitId,
                                LineCode = "1",
                                TrxSource = SourceJournal.Amortization,
                                //IsJobClosed = "Y",
                                OriginalAmount = (x.Debit ?? 0) != 0 ? x.Debit : x.Credit,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            });

                        await context.PhoenixAddRangeAsync(createJournal.ToArray());
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

        public async Task<List<Amortization>> GetList(Amortization model)
        {
            var result = await (from a in context.Amortizations
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                where !(a.IsDeleted ?? false) &&
                                (string.IsNullOrEmpty(model.LegalEntityId) ? true : a.LegalEntityId == model.LegalEntityId) &&
                                ((model.AmotizationDate != null && model.AmotizationDateEnd != null) ? (a.AmotizationDate >= model.AmotizationDate && a.AmotizationDate <= model.AmotizationDateEnd) : true) &&
                                (string.IsNullOrEmpty(model.Code) ? true : a.Code == model.Code) &&
                                (string.IsNullOrEmpty(model.CurrencyId) ? true : a.CurrencyId == model.CurrencyId) &&
                                ((model.ExchangeRate == null) ? true : a.ExchangeRate == model.ExchangeRate) &&
                                (string.IsNullOrEmpty(model.AmotizationDescription) ? true : a.AmotizationDescription == model.AmotizationDescription)
                                select new Amortization
                                {
                                    Id = a.Id,
                                    AmotizationDate = a.AmotizationDate,
                                    Code = a.Code,
                                    //JvReferenceNo = a.JvReferenceNo,
                                    AmotizationDescription = a.AmotizationDescription,
                                    LegalEntityName = b.LegalEntityName,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == a.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliationId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    IsDraft = a.IsDraft,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public Task<List<Amortization>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Amortization> Get(string Id) => await context.Amortizations.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<List<AmortizationDetail>> GetDetailJournal(string costSharingAllocationId, string condition, decimal amount, string id)
        {
            if (string.IsNullOrEmpty(id) || context.Amortizations.Where(x => x.Id == id && x.CostSharingAllocationId == costSharingAllocationId).Count() == 0)
            {
                return await (from a in context.CostSharingAllocationDetails
                              join b in context.BusinessUnits on a.BusinessUnitId equals b.Id
                              where a.CostSharingAllocationId == costSharingAllocationId
                              select new AmortizationDetail
                              {
                                  Id = Guid.NewGuid().ToString(),
                                  //AmortizationId = a.AmortizationId,
                                  //CoaId = a.CoaId,
                                  //Description = a.Description,
                                  Debit = condition == "Credit" ? amount * (a.CostSharingAllocationPercent / 100) : 0,
                                  Credit = condition == "Debit" ? amount * (a.CostSharingAllocationPercent / 100) : 0,
                                  //CodeRec = b.CodeRec,
                                  //AccountName = b.Name5,
                                  Condition = condition == "Debit" ? "Credit" : "Debit",
                                  //CreatedBy = a.CreatedBy,
                                  //CreatedOn = a.CreatedOn,
                                  BusinessUnitId = a.BusinessUnitId,
                                  AffiliationId = b.AffiliationId,
                                  BusinessUnitName = b.UnitName
                              }).ToListAsync();
            }
            else
            {
                var amortizon = await context.Amortizations.Where(x => x.Id == id && x.CostSharingAllocationId == costSharingAllocationId).FirstOrDefaultAsync();
                return await (from a in context.AmortizationDetails
                              join b in context.ChartOfAccounts on a.CoaId equals b.Id
                              join c in context.BusinessUnits on a.BusinessUnitId equals c.Id
                              join d in context.Amortizations on a.AmortizationId equals d.Id
                              where a.AmortizationId == id
                              select new AmortizationDetail
                              {
                                  Id = a.Id,
                                  AmortizationId = a.AmortizationId,
                                  CoaId = a.CoaId,
                                  Description = a.Description,
                                  Debit = (amortizon.Amount == amount && d.Condition == condition) ? a.Debit : condition == "Credit" ? amount * ((context.CostSharingAllocationDetails.Where(x => !(x.IsDeleted ?? false) && x.CostSharingAllocationId == costSharingAllocationId && x.BusinessUnitId == a.BusinessUnitId).Select(x => x.CostSharingAllocationPercent).FirstOrDefault() ?? 0) / 100) : 0,
                                  Credit = (amortizon.Amount == amount && d.Condition == condition) ? a.Credit : condition == "Debit" ? amount * ((context.CostSharingAllocationDetails.Where(x => !(x.IsDeleted ?? false) && x.CostSharingAllocationId == costSharingAllocationId && x.BusinessUnitId == a.BusinessUnitId).Select(x => x.CostSharingAllocationPercent).FirstOrDefault() ?? 0) / 100) : 0,
                                  CodeRec = b.CodeRec,
                                  AccountName = b.Name5,
                                  Condition = amortizon.Condition == condition ? a.Condition : amortizon.Condition == "Debit" ? "Credit" : "Debit",
                                  CreatedBy = a.CreatedBy,
                                  CreatedOn = a.CreatedOn,
                                  BusinessUnitId = a.BusinessUnitId,
                                  AffiliationId = a.AffiliationId,
                                  BusinessUnitName = c.UnitName
                              }).ToListAsync();
            }
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = new List<Journal>();
            if (context.Amortizations.Where(x => x.Id == Id && x.IsDraft == false).Count() > 0)
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

        public Task<List<Amortization>> Get()
        {
            throw new NotImplementedException();
        }
    }
}