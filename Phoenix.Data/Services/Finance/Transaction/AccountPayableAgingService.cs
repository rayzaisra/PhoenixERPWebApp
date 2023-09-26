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
    public interface IAccountPayableAgingService
    {
        Task<List<VwAgingAp>> Get(VwAgingApFilter Filter);
        Task<JournalWriteOffAp> Get(string reffNo);
        Task<List<Journal>> GetJournal(string Id);
        Task<int> AddAsync(JournalWriteOffAp entity);
        Task<int> EditAsync(JournalWriteOffAp entity);
        Task<int> CreateJournal(JournalWriteOffAp entity);
        Task<int> EditJournal(JournalWriteOffAp entity);
        Task<int> DeleteSoftAsync(string Id);
    }
    public class AccountPayableAgingService : IAccountPayableAgingService
    {
        readonly DataContext context;
        public AccountPayableAgingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<VwAgingAp>> Get(VwAgingApFilter Filter)
        {
            var result = await context.VwAgingAp
                        .Where(x => (string.IsNullOrEmpty(Filter.VendorId) ? true : x.VendorId == Filter.VendorId) &&
                            (string.IsNullOrEmpty(Filter.LegalEntityId) ? true : x.LegalEntityId == Filter.LegalEntityId) &&
                            (string.IsNullOrEmpty(Filter.MainserviceCategoryId) ? true : x.MainserviceCategoryId == Filter.MainserviceCategoryId) &&
                            (string.IsNullOrEmpty(Filter.JobId) ? true : x.JobId == Filter.JobId) &&
                            (string.IsNullOrEmpty(Filter.TypeOfExpenseId) ? true : x.TypeOfExpenseId == Filter.TypeOfExpenseId) &&
                            (string.IsNullOrEmpty(Filter.CurrencyId) ? true : x.CurrencyId == Filter.CurrencyId) &&
                            (Filter.OriginalAmount == null ? true : x.Amount == Filter.OriginalAmount)
                        ).ToListAsync();
            return result;
        }

        public async Task<JournalWriteOffAp> Get(string reffNo)
        {
            var result = await context.JournalWriteOffAp.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JvReferenceNo == reffNo).FirstOrDefaultAsync();
            if (result == null)
            {
                var ap = await context.Journal.Where(x => x.ReferenceId == reffNo && x.LineCode == "1" && x.TrxSource == SourceJournal.AccountPayable).FirstOrDefaultAsync() ?? new Journal();
                var woAmount = context.JournalWriteOffAp.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JvReferenceNo == reffNo && x.IsDraft == false).Sum(x => x.Amount);
                result = new JournalWriteOffAp()
                {
                    JobId = ap.JobId,
                    JvDate = DateTime.Now,
                    CurrencyId = ap.CurrencyId,
                    ExchangeRate = ap.ExchangeRate,
                    JvReferenceNo = ap.ReferenceId,
                    LegalEntityId = ap.LegalId,
                    BusinessUnitId = ap.DivisiId,
                    AffiliationId = ap.AffiliateId,
                    Amount = ap.OriginalAmount - woAmount,//(ap.Credit ?? ap.Debit) - woAmount,
                    ReceivableAccount = ap.AccountId,
                    IsDraft = true
                };
            }
            return result;
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = new List<Journal>();
            if (context.JournalWriteOffAp.Where(x => !(x.IsDeleted ?? false) && x.Id == Id && x.IsDraft == false).Count() > 0)
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

        public async Task<int> AddAsync(JournalWriteOffAp entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(JournalWriteOffAp entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournal(JournalWriteOffAp entity)
        {
            return await AddEdit(entity, false, true);
        }

        public async Task<int> EditJournal(JournalWriteOffAp entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(JournalWriteOffAp entity, bool isEdit, bool isJournal)
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
                        entity.Id = await TransID.GetTransId(context, Code.JournalWriteOffAp, DateTime.Today);
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
                        var poId = await (from a in context.AccountPayables.AsNoTracking()
                                          join b in context.InvoiceReceivables.AsNoTracking() on a.InvoiceReceivableId equals b.Id
                                          where a.AccountPayableNumber == entity.JvReferenceNo
                                          select b.PoId).FirstOrDefaultAsync();

                        var createJournalWriteOff = new JournalTemp
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = 0,
                            Credit = entity.Amount * entity.ExchangeRate,
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
                            TrxSource = SourceJournal.JournalWriteOffAp,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PoId = poId,
                            PcaId = pcaId,
                            PceId = pceId
                        };
                        await context.PhoenixAddAsync(createJournalWriteOff);

                        var createJournalAP = new JournalTemp
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = entity.Amount * entity.ExchangeRate,
                            Credit = 0,
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
                            TrxSource = SourceJournal.JournalWriteOffAp,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PoId = poId,
                            PcaId = pcaId,
                            PceId = pceId
                        };
                        await context.PhoenixAddAsync(createJournalAP);
                    }

                    if (!(entity.IsDraft ?? true))
                    {
                        var pcaId = await context.Pcas.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();
                        var pceId = await context.Pces.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JobId == entity.JobId).Select(x => x.Id).FirstOrDefaultAsync();
                        var poId = await (from a in context.AccountPayables.AsNoTracking()
                                          join b in context.InvoiceReceivables.AsNoTracking() on a.InvoiceReceivableId equals b.Id
                                          where a.AccountPayableNumber == entity.JvReferenceNo
                                          select b.PoId).FirstOrDefaultAsync();

                        var createJournalWriteOff = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = 0,
                            Credit = entity.Amount * entity.ExchangeRate,
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
                            TrxSource = SourceJournal.JournalWriteOffAp,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PoId = poId,
                            PcaId = pcaId,
                            PceId = pceId
                        };
                        await context.PhoenixAddAsync(createJournalWriteOff);

                        var createJournalAP = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = entity.Amount * entity.ExchangeRate,
                            Credit = 0,
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
                            TrxSource = SourceJournal.JournalWriteOffAp,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            JobId = entity.JobId,
                            PoId = poId,
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
            var get = await context.JournalWriteOffAp.AsNoTracking().Where(x => x.Id == Id).FirstOrDefaultAsync();
            context.PhoenixDelete(get);

            var journalList = await context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == Id).ToListAsync();
            context.PhoenixDeleteRange(journalList.ToArray());
            return await context.SaveChangesAsync();
        }
    }
}
