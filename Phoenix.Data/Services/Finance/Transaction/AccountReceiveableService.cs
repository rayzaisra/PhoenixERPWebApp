using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IAccountReceiveableService
    {
        Task<List<InvoiceClient>> GetARList(FilterInvoiceClient search);
        Task<InvoiceClient> Get(string id);
        Task<int> EditAsync(InvoiceClient entity);
        Task<List<Journal>> GetDataJournal(string Id);
    }

    public class AccountReceiveableService : IAccountReceiveableService
    {
        readonly DataContext context;
        readonly FinancePeriodService financePeriod;
        readonly FileService uploadFile;
        public AccountReceiveableService(DataContext context, FinancePeriodService financePeriod, FileService uploadFile)
        {
            this.context = context;
            this.financePeriod = financePeriod;
            this.uploadFile = uploadFile;
        }

        public async Task<List<InvoiceClient>> GetARList(FilterInvoiceClient search)
        {
            var month = (search.EndingDate.HasValue ? search.EndingDate.Value.Month : 0);
            var year = (search.EndingDate.HasValue ? search.EndingDate.Value.Year : 0);

            var bankReceived = (from a in context.BankReceived
                                join b in context.BankReceivedDetail on a.Id equals b.BankReceivedId
                                join c in context.Currencies on a.CurrencyId equals c.Id
                                where !(a.IsDeleted ?? false)
                                select new { a.Id, a.Status, b.AccountReceivebleNumber, b.Amount, c.CurrencySymbol });

            var singlePce = await (from b in context.AccountReceivables
                                   join a in context.InvoiceClients on b.AccountReceivableNumber equals a.Id
                                   join e in context.Companys on a.ClientId equals e.Id
                                   join g in context.LegalEntity on a.LegalId equals g.Id
                                   join i in context.Banks on a.AccountId equals i.Id
                                   join j in context.MainServiceCategorys on a.MainServiceCategory equals j.Id
                                   join h in context.CompanyBrands on a.BrandId equals h.Id
                                   join k in context.Pces on a.PceId equals k.Id
                                   where a.MultiPceId == null
                                   && !(a.IsDeleted ?? false) && (string.IsNullOrEmpty(search.Status) ? true : a.Status == search.Status)
                                   && (search.DateType == "Day" ? (search.EndingDate == null ? true : a.InvoiceDate == search.EndingDate) :
                                    search.DateType == "Month" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year && (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Month : 0) == month) :
                                    search.DateType == "Year" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year) :
                                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : a.InvoiceDate >= search.StartingDate && a.InvoiceDate <= search.EndingDate) : true)
                                   select new InvoiceClient
                                   {
                                       Id = a.Id,
                                       PceId = a.PceId,
                                       InvoiceDate = a.InvoiceDate,
                                       JobName = $"{a.JobId} - {a.JobName}",
                                       ClientName = e.CompanyName,
                                       //GrandTotal = b.Amount,
                                       GrandTotal = a.GrandTotal,
                                       LegalEntityName = g.LegalEntityName,
                                       DivisionName = context.BusinessUnits.Where(x => x.Id == a.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                       Status = b.Status,
                                       MultiPceId = a.MultiPceId,
                                       CurrencySymbol = context.Currencies.Where(x => x.Id == b.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                       MainServiceCategoryName = j.Name,
                                       ClientBrandName = h.BrandName,
                                       ClientTaxNumber = e.TaxRegistrationNumber,
                                       ExchangeRate = a.ExchangeRate,
                                       POClientNo = k.PoClient,
                                       CurrencySymbolDefault = (from o in context.JournalTemps
                                                                join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                                where o.ReferenceId == a.Id
                                                                select p.CurrencySymbol).FirstOrDefault() ??
                                                                context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                       BankReceivedNumber = bankReceived.Where(x => x.AccountReceivebleNumber == b.AccountReceivableNumber).Select(x => x.Id).FirstOrDefault(),
                                       BankReceivedAmount = bankReceived.Where(x => x.AccountReceivebleNumber == b.AccountReceivableNumber).Select(x => x.Amount).FirstOrDefault(),
                                       BankReceivedStatus = bankReceived.Where(x => x.AccountReceivebleNumber == b.AccountReceivableNumber).Select(x => x.Status).FirstOrDefault(),
                                       BankReceivedCurrencySymbol = bankReceived.Where(x => x.AccountReceivebleNumber == b.AccountReceivableNumber).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                   }).ToListAsync();

            var multiplePCE = await (from b in context.AccountReceivables
                                     join a in context.InvoiceClients on b.AccountReceivableNumber equals a.MultiPceId
                                     join e in context.Companys on a.ClientId equals e.Id
                                     join g in context.LegalEntity on a.LegalId equals g.Id
                                     join j in context.MainServiceCategorys on a.MainServiceCategory equals j.Id
                                     //join h in context.CompanyBrands on a.BrandId equals h.Id
                                     //join k in context.Pces on a.PceId equals k.Id
                                     where a.MultiPceId != null
                                     && !(a.IsDeleted ?? false) && (string.IsNullOrEmpty(search.Status) ? true : a.Status == search.Status)
                                     && (search.DateType == "Day" ? (search.EndingDate == null ? true : a.InvoiceDate == search.EndingDate) :
                                    search.DateType == "Month" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year && (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Month : 0) == month) :
                                    search.DateType == "Year" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year) :
                                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : a.InvoiceDate >= search.StartingDate && a.InvoiceDate <= search.EndingDate) : true)
                                     group a by new { a.MultiPceId, a.InvoiceDate, b.Status, b.CurrencyId, e.CompanyName, g.LegalEntityName, MSCName = j.Name, e.TaxRegistrationNumber, a.ExchangeRate, b.AccountReceivableNumber, a.BrandId, a.Vat } into g //a.Vat
                                     select new InvoiceClient
                                     {
                                         Id = g.Key.MultiPceId,
                                         PceId = String.Join(",<br/>", g.Select(x => x.PceId).Distinct().ToArray()),
                                         InvoiceDate = g.Key.InvoiceDate,
                                         JobName = String.Join(",<br/>", g.Select(x => $"{x.JobId} - {x.JobName}").Distinct().ToArray()),
                                         ClientName = g.Key.CompanyName,
                                         GrandTotal = g.Sum(x => (x.GrandTotal ?? 0)) + (g.Key.Vat ?? 0),
                                         //GrandTotal = g.Sum(x => (x.GrandTotal ?? 0)),
                                         LegalEntityName = g.Key.LegalEntityName,
                                         DivisionName = String.Join(",<br/>", context.BusinessUnits.Where(x => g.Any(z => z.BusinessUnitId == x.Id)).Select(x => x.UnitName).Distinct().ToArray()),
                                         Status = g.Key.Status,
                                         MultiPceId = g.Key.MultiPceId,
                                         CurrencySymbol = context.Currencies.Where(x => x.Id == g.Key.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                         MainServiceCategoryName = g.Key.MSCName,
                                         ClientBrandName = String.Join(",<br/>", context.CompanyBrands.Where(x => g.Key.BrandId.Contains(x.Id)).Select(x => x.BrandName).Distinct().ToArray()),
                                         ClientTaxNumber = g.Key.TaxRegistrationNumber,
                                         ExchangeRate = g.Key.ExchangeRate,
                                         POClientNo = String.Join(",<br/>", context.Pces.Where(x => g.Select(y => y.PceId).Any(z => z == x.Id)).Select(x => x.PoClient).Distinct().ToArray()),
                                         CurrencySymbolDefault = (from o in context.JournalTemps
                                                                  join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                                  where o.ReferenceId == g.Key.MultiPceId
                                                                  select p.CurrencySymbol).FirstOrDefault() ??
                                                                context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                         BankReceivedNumber = bankReceived.Where(x => x.AccountReceivebleNumber == g.Key.AccountReceivableNumber).Select(x => x.Id).FirstOrDefault(),
                                         BankReceivedAmount = bankReceived.Where(x => x.AccountReceivebleNumber == g.Key.AccountReceivableNumber).Select(x => x.Amount).FirstOrDefault(),
                                         BankReceivedStatus = bankReceived.Where(x => x.AccountReceivebleNumber == g.Key.AccountReceivableNumber).Select(x => x.Status).FirstOrDefault(),
                                         BankReceivedCurrencySymbol = bankReceived.Where(x => x.AccountReceivebleNumber == g.Key.AccountReceivableNumber).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                     }).ToListAsync();
            if (singlePce.Count() > 0)
            {
                return singlePce.Union(multiplePCE).ToList();
            }
            else
            {
                return multiplePCE;
            }
        }

        public async Task<InvoiceClient> Get(string id)
        {
            var isMultiplePce = (from a in context.AccountReceivables
                                 join b in context.InvoiceClients on a.AccountReceivableNumber equals b.MultiPceId
                                 where b.MultiPceId == id
                                 select b).Count() > 1;
            if (isMultiplePce)
            {
                var result = await (from a in context.AccountReceivables
                                    join z in context.InvoiceClients on a.AccountReceivableNumber equals z.MultiPceId
                                    join c in context.Currencies on z.CurrencyId equals c.Id
                                    join d in context.MainServiceCategorys on z.MainServiceCategory equals d.Id
                                    join e in context.LegalEntity on z.LegalId equals e.Id
                                    //join h in context.CompanyBrands on z.BrandId equals h.Id
                                    join i in context.Companys on z.ClientId equals i.Id
                                    where z.MultiPceId == id && z.MultiPceId != null
                                    select new InvoiceClient
                                    {
                                        Id = z.Id,
                                        PceId = z.PceId,
                                        PceSeq = z.PceSeq,
                                        BrandId = z.BrandId,
                                        ClientId = z.ClientId,
                                        AlternateClientId = z.AlternateClientId,
                                        CurrencyId = z.CurrencyId,
                                        InvoiceDate = z.InvoiceDate,
                                        InvoiceNumber = z.InvoiceNumber,
                                        StartPeriod = z.StartPeriod,
                                        EndPeriod = z.EndPeriod,
                                        VatNo = z.VatNo,
                                        Vat = z.Vat,
                                        ExchangeRate = z.ExchangeRate,
                                        JobName = context.JobDetail.Where(x => x.Id == z.JobId).Select(x => x.JobName).FirstOrDefault(),//z.JobName,
                                        JobId = z.JobId,
                                        Remarks = z.Remarks,
                                        EstimateNumber = z.EstimateNumber,
                                        RefNumber = z.RefNumber,
                                        YourRef = z.YourRef,
                                        Status = a.Status,
                                        AccountId = z.AccountId,
                                        MultiPceId = z.MultiPceId,
                                        PayDescription = z.PayDescription,
                                        PoDocFileId = z.PoDocFileId,
                                        VatDocFileId = z.VatDocFileId,
                                        MainServiceCategory = z.MainServiceCategory,
                                        LegalId = z.LegalId,
                                        AffiliationId = z.AffiliationId,
                                        BusinessUnitId = z.BusinessUnitId,
                                        POClientNo = z.POClientNo,
                                        InvoiceBilled = z.InvoiceBilled,
                                        CreatedBy = z.CreatedBy,
                                        CreatedOn = z.CreatedOn,
                                        LegalEntityName = e.LegalEntityName,
                                        //TermOfPayment = h.TermOfPayment,
                                        JobStatus = context.JobDetail.Where(x => x.Id == z.JobId).Select(x => x.JobStatusId).FirstOrDefault(),
                                        ClientName = i.CompanyName,
                                        //ClientBrandName = h.BrandName,
                                        //ClientInvAddress = h.BrandAddress,
                                        ClientTaxNumber = i.TaxRegistrationNumber,
                                        //ClientPIC = h.ContactName,
                                        TotalAmount = context.InvoiceClients.Where(x => !(x.IsDeleted ?? false) && x.MultiPceId == z.MultiPceId).Sum(x => x.GrandTotal),
                                        VAtPercentage = Math.Ceiling((decimal)((z.Vat / (context.InvoiceClients.Where(x => !(x.IsDeleted ?? false) && x.MultiPceId == z.MultiPceId).Sum(x => x.GrandTotal) + (z.AsfAmount ?? 0))) * 100)),
                                        GrandTotal = context.InvoiceClients.Where(x => !(x.IsDeleted ?? false) && x.MultiPceId == z.MultiPceId).Sum(x => x.GrandTotal) + (z.Vat ?? 0),
                                        AccountBankNumber = context.Banks.Where(x => x.Id == z.AccountId).Select(x => x.AccountNumber).FirstOrDefault(),
                                        InvoiceTo = $"{z.PceSeq} of {context.InvoiceClients.Where(x => !(x.IsDeleted ?? false) && x.PceId == z.PceId).Max(x => x.PceSeq)}",
                                        InvoiceDeliveryDate = a.InvoiceDeliveryDate,
                                        InvoiceDeliveryNo = a.InvoiceDeliveryNotesId,
                                        VatExchangeRate = z.VatExchangeRate,
                                        CurrencySymbol = c.CurrencySymbol,
                                        CurrencyIdDefault = (context.JournalTemps.Where(x => x.ReferenceId == z.Id).Select(x => x.DefaultCurrencyId)).FirstOrDefault() ??
                                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                        CurrencySymbolDefault = (from o in context.JournalTemps
                                                                 join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                                 where o.ReferenceId == z.Id
                                                                 select p.CurrencySymbol).FirstOrDefault() ??
                                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault()
                                    }).AsNoTracking().FirstOrDefaultAsync();
                return await GetFile(result);
            }
            else
            {
                var result = await (from a in context.AccountReceivables
                                    join z in context.InvoiceClients on a.AccountReceivableNumber equals z.Id
                                    join c in context.Currencies on z.CurrencyId equals c.Id
                                    join d in context.MainServiceCategorys on z.MainServiceCategory equals d.Id
                                    join e in context.LegalEntity on z.LegalId equals e.Id
                                    join h in context.CompanyBrands on z.BrandId equals h.Id
                                    join i in context.Companys on z.ClientId equals i.Id
                                    where z.Id == id && z.MultiPceId == null
                                    select new InvoiceClient
                                    {
                                        Id = z.Id,
                                        PceId = z.PceId,
                                        PceSeq = z.PceSeq,
                                        BrandId = z.BrandId,
                                        ClientId = z.ClientId,
                                        AlternateClientId = z.AlternateClientId,
                                        CurrencyId = z.CurrencyId,
                                        InvoiceDate = z.InvoiceDate,
                                        InvoiceNumber = z.InvoiceNumber,
                                        StartPeriod = z.StartPeriod,
                                        EndPeriod = z.EndPeriod,
                                        VatNo = z.VatNo,
                                        Vat = z.Vat,
                                        VatExchangeRate = z.VatExchangeRate,
                                        TotalAmount = z.TotalAmount,
                                        ExchangeRate = z.ExchangeRate,
                                        JobName = context.JobDetail.Where(x => x.Id == z.JobId).Select(x => x.JobName).FirstOrDefault(),//z.JobName,
                                        JobId = z.JobId,
                                        Remarks = z.Remarks,
                                        EstimateNumber = z.EstimateNumber,
                                        RefNumber = z.RefNumber,
                                        YourRef = z.YourRef,
                                        Status = a.Status,
                                        AccountId = z.AccountId,
                                        Asf = z.Asf,
                                        AsfAmount = z.AsfAmount,
                                        GrandTotal = z.GrandTotal,
                                        MultiPceId = z.MultiPceId,
                                        PayDescription = z.PayDescription,
                                        PoDocFileId = z.PoDocFileId,
                                        VatDocFileId = z.VatDocFileId,
                                        MainServiceCategory = z.MainServiceCategory,
                                        LegalId = z.LegalId,
                                        AffiliationId = z.AffiliationId,
                                        BusinessUnitId = z.BusinessUnitId,
                                        POClientNo = z.POClientNo,
                                        InvoiceBilled = z.InvoiceBilled,
                                        CreatedBy = z.CreatedBy,
                                        CreatedOn = z.CreatedOn,
                                        LegalEntityName = e.LegalEntityName,
                                        TermOfPayment = h.TermOfPayment,
                                        JobStatus = context.JobDetail.Where(x => x.Id == z.JobId).Select(x => x.JobStatusId).FirstOrDefault(),
                                        ClientName = i.CompanyName,
                                        ClientBrandName = h.BrandName,
                                        ClientInvAddress = h.BrandAddress,
                                        ClientTaxNumber = i.TaxRegistrationNumber,
                                        ClientPIC = h.ContactName,
                                        totalamtplusasf = z.TotalAmount + (z.AsfAmount ?? 0),
                                        VAtPercentage = Math.Ceiling((decimal)((z.Vat / (z.TotalAmount + (z.AsfAmount ?? 0))) * 100)),
                                        PceBalance = string.IsNullOrEmpty(z.PceId) ? (decimal)0 : context.Pces.Where(x => x.Id == z.PceId).Select(x => x.SubtotalOriginal ?? x.Subtotal).FirstOrDefault() - z.InvoiceBilled,
                                        AccountBankNumber = context.Banks.Where(x => x.Id == z.AccountId).Select(x => x.AccountNumber).FirstOrDefault(),
                                        InvoiceTo = $"{z.PceSeq} of {context.InvoiceClients.Where(x => x.PceId == z.PceId).Max(x => x.PceSeq)}",
                                        TotalPCE = context.Pces.Where(x => x.Id == z.PceId).Select(x => x.SubtotalOriginal ?? x.Subtotal).FirstOrDefault(),
                                        InvoiceDeliveryDate = a.InvoiceDeliveryDate,
                                        InvoiceDeliveryNo = a.InvoiceDeliveryNotesId,
                                        CurrencySymbol = c.CurrencySymbol,
                                        CurrencyIdDefault = (context.JournalTemps.Where(x => x.ReferenceId == z.Id).Select(x => x.DefaultCurrencyId)).FirstOrDefault() ??
                                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                        CurrencySymbolDefault = (from o in context.JournalTemps
                                                                 join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                                 where o.ReferenceId == z.Id
                                                                 select p.CurrencySymbol).FirstOrDefault() ??
                                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault()
                                    }).AsNoTracking().FirstOrDefaultAsync();
                return await GetFile(result);
            }
        }

        private async Task<InvoiceClient> GetFile(InvoiceClient data)
        {
            if (data.PoDocFileId != null)
            {
                var a = await context.Filemasters.Where(x => x.Id == data.PoDocFileId).FirstOrDefaultAsync();
                if (a != null) data.PoDocFileName = a.Name;
            }

            if (data.VatDocFileId != null)
            {
                var b = await context.Filemasters.Where(x => x.Id == data.VatDocFileId).FirstOrDefaultAsync();
                if (b != null) data.VatDocFileName = b.Name;
            }
            return data;
        }

        public async Task<int> EditAsync(InvoiceClient entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {

                    var model = await ProcessUpload(entity);
                    if (!string.IsNullOrWhiteSpace(model.MultiPceId))
                    {
                        var ARList = await context.AccountReceivables.Where(b => b.AccountReceivableNumber == model.MultiPceId).AsNoTracking().ToListAsync();
                        foreach (var item in ARList)
                        {
                            item.Status = ARStatus.Posted;
                            item.VatNo = model.VatNo;
                            context.PhoenixEdit(item);
                        }
                    }
                    else
                    {
                        var AR = context.AccountReceivables.AsNoTracking().Where(x => x.Id == model.Id).FirstOrDefault();
                        AR.Status = ARStatus.Posted;
                        AR.VatNo = model.VatNo;
                        context.PhoenixEdit(AR);
                    }

                    context.Database.ExecuteSqlCommand($"UPDATE fn.invoice_client SET vat_no={model.VatNo}, vat_doc_file_id= {entity.VatDocFileId}  WHERE multi_pce_id={model.MultiPceId}");
                    context.Database.ExecuteSqlCommand($"UPDATE fn.invoice_client SET vat_no={model.VatNo}, vat_doc_file_id= {entity.VatDocFileId} WHERE id={model.Id}");

                    context.Database.ExecuteSqlCommand($"UPDATE fn.e_faktur_detail SET is_used=0, reff_number=NULL WHERE reff_number={model.InvoiceNumber}");
                    context.Database.ExecuteSqlCommand($"UPDATE fn.e_faktur_detail SET is_used=1, reff_number={model.InvoiceNumber} WHERE vat_no={model.VatNo}");

                    await context.SaveChangesAsync();
                    await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.store_data_jurnal @p_id", parameters: new[] { new SqlParameter("@p_id", !string.IsNullOrEmpty(model.MultiPceId) ? model.MultiPceId : model.Id) });

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public async Task<List<Journal>> GetDataJournal(string Id)
        {
            var isMultiplePce = (from a in context.AccountReceivables
                                 join b in context.InvoiceClients on a.AccountReceivableNumber equals b.MultiPceId
                                 where b.MultiPceId == Id
                                 select b).Count() > 1;
            if (isMultiplePce)
            {
                var result = await (from a in context.InvoiceClients
                                    join b in context.JournalTemps on a.MultiPceId equals b.ReferenceId
                                    join c in context.ChartOfAccounts on b.AccountId equals c.Id
                                    where a.MultiPceId == Id && !(a.IsDeleted ?? false)
                                    group new { a, b, c } by new { a.MultiPceId, coaId = c.Id, c.CodeRec, c.Name5, b.Debit, b.Credit, b.DefaultCurrencyId } into g
                                    select new Journal
                                    {
                                        AccountId = g.Key.coaId,
                                        CodeRec = g.Key.CodeRec,
                                        AccountName = g.Key.Name5,
                                        Description = String.Join(",<br/>", g.Select(x => x.b.Description).Distinct().ToArray()),
                                        JobIdName = String.Join(",<br/>", g.Select(x => x.a.JobName).Distinct().ToArray()),
                                        DivisiName = String.Join(",<br/>", context.BusinessUnits.Where(x => g.Any(z => z.a.BusinessUnitId == x.Id)).Select(x => x.UnitName).Distinct().ToArray()),
                                        LegalEntityName = String.Join(",<br/>", context.LegalEntity.Where(x => g.Any(z => z.a.LegalId == x.Id)).Select(x => x.LegalEntityName).Distinct().ToArray()),
                                        Credit = g.Key.Credit,
                                        Debit = g.Key.Debit,
                                        CurrencySymbol = context.Currencies.Where(x => x.Id == g.Key.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                    }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
                return result;
            }
            else
            {
                var result = await (from tb in context.JournalTemps
                                    join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                    join inv in context.InvoiceClients on tb.ReferenceId equals inv.Id
                                    where tb.ReferenceId == Id
                                    select new Journal
                                    {
                                        AccountId = coa.Id,
                                        CodeRec = coa.CodeRec,
                                        AccountName = coa.Name5,
                                        JobId = tb.JobId,
                                        Description = tb.Description,
                                        JobIdName = $"{inv.JobId} - {inv.JobName}",
                                        DivisiName = context.BusinessUnits.Where(x => x.Id == tb.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                        LegalEntityName = context.LegalEntity.Where(x => x.Id == tb.LegalId).Select(x => x.LegalEntityName).FirstOrDefault(),
                                        Credit = tb.Credit,
                                        Debit = tb.Debit,
                                        CurrencySymbol = context.Currencies.Where(x => x.Id == tb.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                    }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
                return result;
            }
        }

        private async Task<InvoiceClient> ProcessUpload(InvoiceClient entity)
        {
            var file1 = await uploadFile.Upload(entity.VatDocFile, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.VatDocFileId = file1;
            }

            return entity;
        }

    }
}
