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
    public interface IJournalEntryService : IDataService<JournalEntry>
    {
        Task<List<JournalEntry>> GetList(JournalEntry model);
        Task<List<JournalEntryDetail>> GetDetailJournal(string Id);
        Task<List<Journal>> GetJournal(string Id);
        Task<int> CreateJournal(JournalEntry entity);
        Task<int> EditJournal(JournalEntry entity);
    }

    public class JournalEntryService : IJournalEntryService
    {
        readonly DataContext context;
        readonly FinancePeriodService financePeriod;
        /// <summary>
        /// And endpoint to manage JournalEntry
        /// </summary>
        /// <param name="context">Database context</param>
        public JournalEntryService(DataContext context, FinancePeriodService financePeriod)
        {
            this.context = context;
            this.financePeriod = financePeriod;
        }

        public async Task<int> AddAsync(JournalEntry entity)
        {
            return await AddEdit(entity, false, false);
        }

        public Task<int> AddRangeAsync(params JournalEntry[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(JournalEntry entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params JournalEntry[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(JournalEntry entity)
        {
            context.PhoenixDelete(entity);

            var journalDetailList = await context.JournalEntryDetails.AsNoTracking().Where(x => x.JournalEntryId == entity.Id).ToListAsync();
            context.PhoenixDeleteRange(journalDetailList.ToArray());

            var journalList = await context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == entity.Id).ToListAsync();
            context.PhoenixDeleteRange(journalList.ToArray());
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params JournalEntry[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(JournalEntry entity)
        {
            return await AddEdit(entity, true, false);
        }

        public Task<int> EditRangeAsync(JournalEntry entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateJournal(JournalEntry entity)
        {
            return await AddEdit(entity, false, true);
        }

        public async Task<int> EditJournal(JournalEntry entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(JournalEntry entity, bool isEdit, bool isJournal)
        {
            if (isJournal)
            {
                await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.JvDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.JE);
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, Code.JournalEntry, DateTime.Today);
                        entity.Code = await TransID.GetTransId(context, Code.JournalVoucher, DateTime.Today);
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.JournalEntryDetails.AsNoTracking().Where(x => x.JournalEntryId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.JournalEntryDetails.RemoveRange(detail);
                        }
                    }

                    if (entity.JournalEntryDetails != null)
                    {
                        foreach (var item in entity.JournalEntryDetails)
                        {
                            item.JournalEntryId = entity.Id;
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

                        var createJournal = entity.JournalEntryDetails
                            .Select(x => new JournalTemp
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = x.Description,
                                Debit = x.Debit * entity.ExchangeRate,
                                Credit = x.Credit * entity.ExchangeRate,
                                ReferenceId = entity.Id,
                                ReferenceNumber = entity.Code,
                                AccountId = x.CoaId,
                                Referencedate = entity.JvDate,
                                TransactionDate = entity.JvDate,
                                CurrencyId = entity.CurrencyId,
                                //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                ExchangeRate = entity.ExchangeRate,
                                LegalId = entity.LegalEntityId,
                                AffiliateId = x.AffiliationId,
                                DivisiId = x.BusinessUnitId,
                                LineCode = "1",
                                TrxSource = SourceJournal.JournalEntry,
                                JobId = x.JobId,
                                PcaId = x.PcaId,
                                PceId = x.PceId,
                                //IsJobClosed = "N",
                                OriginalAmount = (x.Debit ?? 0) != 0 ? x.Debit : x.Credit,
                                DefaultCurrencyId = context.Currencies.AsNoTracking().Where(z => z.IsDefault == true).Select(z => z.Id).FirstOrDefault()
                            });

                        await context.PhoenixAddRangeAsync(createJournal.ToArray());
                    }

                    if (!(entity.IsDraft ?? true))
                    {
                        var createJournal = entity.JournalEntryDetails
                            .Select(x => new Journal
                            {
                                Id = Guid.NewGuid().ToString(),
                                Description = x.Description,
                                Debit = x.Debit * entity.ExchangeRate,
                                Credit = x.Credit * entity.ExchangeRate,
                                ReferenceId = entity.Id,
                                ReferenceNumber = entity.Code,
                                AccountId = x.CoaId,
                                ReferenceDate = entity.JvDate,
                                TransactionDate = entity.JvDate,
                                CurrencyId = entity.CurrencyId,
                                //Unit = context.Currencies.AsNoTracking().Where(z => z.Id == entity.CurrencyId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                ExchangeRate = entity.ExchangeRate,
                                LegalId = entity.LegalEntityId,
                                AffiliateId = x.AffiliationId,
                                DivisiId = x.BusinessUnitId,
                                LineCode = "1",
                                TrxSource = SourceJournal.JournalEntry,
                                JobId = x.JobId,
                                PcaId = x.PcaId,
                                PceId = x.PceId,
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

        public async Task<List<JournalEntry>> GetList(JournalEntry model)
        {
            var result = await (from a in context.JournalEntrys
                                join b in context.LegalEntity on a.LegalEntityId equals b.Id
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                where !(a.IsDeleted ?? false) &&
                                (string.IsNullOrEmpty(model.LegalEntityId) ? true : a.LegalEntityId == model.LegalEntityId) &&
                                ((model.JvDate != null && model.JvDateEnd != null) ? (a.JvDate >= model.JvDate && a.JvDate <= model.JvDateEnd) : true) &&
                                (string.IsNullOrEmpty(model.Code) ? true : a.Code == model.Code) &&
                                (string.IsNullOrEmpty(model.CurrencyId) ? true : a.CurrencyId == model.CurrencyId) &&
                                ((model.ExchangeRate == null) ? true : a.ExchangeRate == model.ExchangeRate) &&
                                (string.IsNullOrEmpty(model.JvDescription) ? true : a.JvDescription == model.JvDescription)
                                select new JournalEntry
                                {
                                    Id = a.Id,
                                    JvDate = a.JvDate,
                                    Code = a.Code,
                                    //JvReferenceNo = a.JvReferenceNo,
                                    JvDescription = a.JvDescription,
                                    LegalEntityName = b.LegalEntityName,
                                    //BusinessUnitName = context.BusinessUnits.Where(x => x.Id == a.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    //AfiliationName = context.Affiliations.Where(x => x.Id == a.AffiliationId).Select(x => x.AffiliationName).FirstOrDefault(),
                                    CurrencyId = c.CurrencyName,
                                    ExchangeRate = a.ExchangeRate,
                                    IsDraft = a.IsDraft,
                                    CreatedOn = a.CreatedOn
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public Task<List<JournalEntry>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<JournalEntry> Get(string Id) => await context.JournalEntrys.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).FirstOrDefaultAsync();

        public async Task<List<JournalEntryDetail>> GetDetailJournal(string Id)
        {
            return await (from a in context.JournalEntryDetails
                          join b in context.ChartOfAccounts on a.CoaId equals b.Id
                          join c in context.BusinessUnits on a.BusinessUnitId equals c.Id
                          where a.JournalEntryId == Id
                          select new JournalEntryDetail
                          {
                              Id = a.Id,
                              JournalEntryId = a.JournalEntryId,
                              CoaId = a.CoaId,
                              Description = a.Description,
                              Condition = a.Condition,
                              Debit = a.Debit,
                              Credit = a.Credit,
                              TransactionDate = a.TransactionDate,
                              JobId = a.JobId,
                              PceId = a.PceId,
                              PcaId = a.PcaId,
                              PcaTaskId = a.PcaTaskId,
                              JobName = context.JobDetail.Where(x => x.Id == a.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
                              PcaTask = context.PcaTasks.Where(x => x.Id == a.PcaTaskId).Select(x => x.TaskName).FirstOrDefault(),
                              //CodeRec = b.CodeRec,
                              AccountName = $"({b.CodeRec}) {b.Name5}",
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn,
                              BusinessUnitId = a.BusinessUnitId,
                              AffiliationId = a.AffiliationId,
                              BusinessUnitName = c.UnitName
                          }).ToListAsync();
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = new List<Journal>();
            if (context.JournalEntrys.Where(x => x.Id == Id && x.IsDraft == false).Count() > 0)
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
                                    CreatedOn = a.CreatedOn,
                                    PceId = a.PceId,
                                    PcaId = a.PcaId,
                                    JobIdName = context.JobDetail.Where(x => x.Id == a.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
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

        public Task<List<JournalEntry>> Get()
        {
            throw new NotImplementedException();
        }
    }
}