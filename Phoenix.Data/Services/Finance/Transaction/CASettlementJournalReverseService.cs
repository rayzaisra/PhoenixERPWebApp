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
    public interface ICASettlementJournalReverseService
    {
        Task<CASettlementJournalReverse> Get(string reffNo);
        Task<List<Journal>> GetJournal(string Id);
        Task<int> AddAsync(CASettlementJournalReverse entity);
        Task<int> EditAsync(CASettlementJournalReverse entity);
        Task<int> CreateJournal(CASettlementJournalReverse entity);
        Task<int> EditJournal(CASettlementJournalReverse entity);
        Task<int> DeleteSoftAsync(string Id);
    }
    public class CASettlementJournalReverseService : ICASettlementJournalReverseService
    {
        readonly DataContext context;
        public CASettlementJournalReverseService(DataContext context)
        {
            this.context = context;
        }

        public async Task<CASettlementJournalReverse> Get(string reffNo)
        {
            var result = await context.CASettlementJournalReverse.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JvReferenceNo == reffNo).FirstOrDefaultAsync();
            if (result == null)
            {
                var ap = await (from a in context.AccountPayables
                                join b in context.PurchaseOrders on a.PurchaseOrderId equals b.Id
                                join c in context.CASettlement on b.CashAdvanceSettlementId equals c.Id
                                join d in context.Journal on a.Id equals d.ReferenceId
                                join e in context.InvoiceReceivables on a.InvoiceReceivableId equals e.Id
                                where d.LineCode == "1"//AP
                                where c.SettlementNumber == reffNo
                                select new
                                {
                                    c.JobId,
                                    a.CurrencyId,
                                    a.ExchangeRate,
                                    ReferenceId = c.SettlementNumber,
                                    LegalId = c.LegalEntityId,
                                    DivisiId = c.BusinessUnitId,
                                    AffiliateId = c.AffiliationId,
                                    OriginalAmount = d.OriginalAmount,
                                    AccountId = d.AccountId,
                                    PONumber = b.PurchaseOrderNumber,
                                    APNumber = a.AccountPayableNumber,
                                    TaxAmount = e.TaxPayable,
                                    TaxAccount = e.TaxPayableCoaId
                                }).FirstOrDefaultAsync();
                if (ap != null)
                {
                    //var defaultCurrencyId = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault();
                    var reverseAmount = context.CASettlementJournalReverse.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.JvReferenceNo == reffNo && x.IsDraft == false).Sum(x => x.Amount);
                    result = new CASettlementJournalReverse()
                    {
                        JobId = ap.JobId,
                        JvDate = DateTime.Now,
                        CurrencyId = ap.CurrencyId,//defaultCurrencyId,
                        ExchangeRate = (ap.ExchangeRate ?? 1),//1,
                        JvReferenceNo = ap.ReferenceId,
                        LegalEntityId = ap.LegalId,
                        BusinessUnitId = ap.DivisiId,
                        AffiliationId = ap.AffiliateId,
                        Amount = ap.OriginalAmount - (reverseAmount ?? 0),
                        ReceivableAccount = ap.AccountId,
                        IsDraft = true,
                        PONumber = ap.PONumber,
                        APNumber = ap.APNumber,
                        TaxAccount = ap.TaxAccount,
                        TaxAmount = ap.TaxAmount
                    };
                }
                else
                {
                    result = new CASettlementJournalReverse();
                }
            }
            return result;
        }

        public async Task<List<Journal>> GetJournal(string Id)
        {
            var result = new List<Journal>();
            if (context.CASettlementJournalReverse.Where(x => !(x.IsDeleted ?? false) && x.Id == Id && x.IsDraft == false).Count() > 0)
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

        public async Task<int> AddAsync(CASettlementJournalReverse entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(CASettlementJournalReverse entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournal(CASettlementJournalReverse entity)
        {
            return await AddEdit(entity, false, true);
        }

        public async Task<int> EditJournal(CASettlementJournalReverse entity)
        {
            return await AddEdit(entity, true, true);
        }

        private async Task<int> AddEdit(CASettlementJournalReverse entity, bool isEdit, bool isJournal)
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
                        entity.Id = await TransID.GetTransId(context, Code.CASettlementJournalReverse, DateTime.Today);
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
                            Debit = 0,
                            Credit = (entity.Amount * entity.ExchangeRate) - (entity.TaxAmount * entity.ExchangeRate),
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
                            TrxSource = SourceJournal.CASettlementJournalReverse,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            PoId = entity.PONumber,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId

                        };
                        await context.PhoenixAddAsync(createJournalWriteOff);

                        var createJournalTax = new JournalTemp
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = 0,
                            Credit = (entity.TaxAmount * entity.ExchangeRate),
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.TaxAccount,
                            Referencedate = entity.JvDate,
                            TransactionDate = entity.JvDate,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "2",
                            TrxSource = SourceJournal.CASettlementJournalReverse,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            PoId = entity.PONumber,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId

                        };
                        await context.PhoenixAddAsync(createJournalTax);

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
                            LineCode = "3",
                            TrxSource = SourceJournal.CASettlementJournalReverse,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            PoId = entity.PONumber,
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
                            Debit = 0,
                            Credit = (entity.Amount * entity.ExchangeRate) - (entity.TaxAmount * entity.ExchangeRate),
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
                            TrxSource = SourceJournal.CASettlementJournalReverse,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            PoId = entity.PONumber,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId

                        };
                        await context.PhoenixAddAsync(createJournalWriteOff);

                        var createJournalTax = new Journal
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = entity.JvDescription,
                            Debit = 0,
                            Credit = (entity.TaxAmount * entity.ExchangeRate),
                            ReferenceId = entity.Id,
                            ReferenceNumber = entity.Code,
                            AccountId = entity.TaxAccount,
                            ReferenceDate = entity.JvDate,
                            TransactionDate = entity.JvDate,
                            CurrencyId = entity.CurrencyId,
                            ExchangeRate = entity.ExchangeRate,
                            LegalId = entity.LegalEntityId,
                            AffiliateId = entity.AffiliationId,
                            DivisiId = entity.BusinessUnitId,
                            LineCode = "2",
                            TrxSource = SourceJournal.CASettlementJournalReverse,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            PoId = entity.PONumber,
                            JobId = entity.JobId,
                            PcaId = pcaId,
                            PceId = pceId

                        };
                        await context.PhoenixAddAsync(createJournalTax);

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
                            LineCode = "3",
                            TrxSource = SourceJournal.CASettlementJournalReverse,
                            //IsJobClosed = "N",
                            OriginalAmount = entity.Amount,
                            DefaultCurrencyId = defaultCurrencyId,
                            PoId = entity.PONumber,
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
            var get = await context.CASettlementJournalReverse.AsNoTracking().Where(x => x.Id == Id).FirstOrDefaultAsync();
            context.PhoenixDelete(get);

            var journalList = await context.JournalTemps.AsNoTracking().Where(x => x.ReferenceId == Id).ToListAsync();
            context.PhoenixDeleteRange(journalList.ToArray());
            return await context.SaveChangesAsync();
        }
    }
}
