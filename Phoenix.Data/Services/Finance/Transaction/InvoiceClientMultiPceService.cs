using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.PrincipalHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IInvoiceClientMultiPceService
    {
        Task<List<InvoiceClient>> GetInvoiceList(FilterInvoiceClient search);
        Task<InvoiceClient> Get(string multipceid);
        Task<List<InvoiceClient>> GetInvoiceDetail(string multipceid);
        Task<List<InvoiceClient>> GetPceList(string clientid, string legalid, string brandid, string mainservice, string currencyid, string divisionid, string invoiceId, bool? pceTypeCrossBilling);
        Task<List<Journal>> GetDataJournal(string Id);
        Task<List<InvoiceClientItemOther>> GetPceTask(string pceid);
        Task<int> AddAsync(InvoiceClient entity);
        Task<int> EditAsync(InvoiceClient entity);
        Task<int> CreateJournalAsync(InvoiceClient entity);
        Task<int> Approve(InvoiceClient entity);
        Task<int> Reject(InvoiceClient entity);
        Task<int> UpdateJournalAsync(Journal entity);
        Task<List<MultipleBrand>> GetMultipleBrandList(string multipceid);
    }

    public class InvoiceClientMultiPceService : IInvoiceClientMultiPceService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly FileService uploadFile;
        readonly StatusLogService log;
        readonly FinancePeriodService financePeriod;
        public InvoiceClientMultiPceService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, FileService uploadFile, StatusLogService log, FinancePeriodService financePeriod)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
            this.log = log;
            this.financePeriod = financePeriod;
        }

        public async Task<List<InvoiceClient>> GetInvoiceList(FilterInvoiceClient search)
        {
            var month = (search.EndingDate.HasValue ? search.EndingDate.Value.Month : 0);
            var year = (search.EndingDate.HasValue ? search.EndingDate.Value.Year : 0);

            var result = await (from a in context.InvoiceClients
                                join e in context.Companys on a.ClientId equals e.Id
                                join g in context.LegalEntity on a.LegalId equals g.Id
                                join j in context.MainServiceCategorys on a.MainServiceCategory equals j.Id
                                //join h in context.CompanyBrands on a.BrandId equals h.Id
                                //join k in context.Pces on a.PceId equals k.Id
                                where a.MultiPceId != null && !(a.IsDeleted ?? false)
                                && (string.IsNullOrEmpty(search.Status) ? true : a.Status == search.Status)
                                && (search.DateType == "Day" ? (search.EndingDate == null ? true : a.InvoiceDate == search.EndingDate) :
                                    search.DateType == "Month" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year && (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Month : 0) == month) :
                                    search.DateType == "Year" ? (search.EndingDate == null ? true : (a.InvoiceDate.HasValue ? a.InvoiceDate.Value.Year : 0) == year) :
                                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : a.InvoiceDate >= search.StartingDate && a.InvoiceDate <= search.EndingDate) : true)
                                group a by new { a.MultiPceId, a.InvoiceDate, a.Status, a.CurrencyId, e.CompanyName, g.LegalEntityName, MSCName = j.Name, e.TaxRegistrationNumber, a.ExchangeRate, a.BrandId, a.Vat } into g  //a.Vat
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
                                }).ToListAsync();
            return result;
        }

        public async Task<InvoiceClient> Get(string multipceid)
        {
            var result = await (from z in context.InvoiceClients
                                join c in context.Currencies on z.CurrencyId equals c.Id
                                join d in context.MainServiceCategorys on z.MainServiceCategory equals d.Id
                                join e in context.LegalEntity on z.LegalId equals e.Id
                                //join h in context.CompanyBrands on z.BrandId equals h.Id
                                join i in context.Companys on z.ClientId equals i.Id
                                where z.MultiPceId == multipceid && z.MultiPceId != null
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
                                    Status = z.Status,
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
                                    PceTypeCrossBilling = z.PceTypeCrossBilling,
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

        public async Task<List<InvoiceClient>> GetInvoiceDetail(string multipceid)
        {
            var result = await (from a in context.InvoiceClients
                                join b in context.Pces on a.PceId equals b.Id
                                join c in context.JobDetail on b.JobId equals c.Id
                                join g in context.CompanyBrands on c.BrandId equals g.Id
                                where b.Status == StatusTransactionName.Approved
                                where a.MultiPceId != null && a.MultiPceId == multipceid
                                select new InvoiceClient
                                {
                                    Id = a.Id,
                                    PceId = a.PceId,
                                    PceSeq = a.PceSeq,
                                    BrandId = c.BrandId,
                                    ClientId = a.ClientId,
                                    AlternateClientId = a.AlternateClientId,
                                    CurrencyId = a.CurrencyId,
                                    InvoiceDate = a.InvoiceDate,
                                    InvoiceNumber = a.InvoiceNumber,
                                    StartPeriod = a.StartPeriod,
                                    EndPeriod = a.EndPeriod,
                                    VatNo = a.VatNo,
                                    Vat = a.Vat,
                                    TotalAmount = a.TotalAmount,
                                    ExchangeRate = a.ExchangeRate,
                                    JobName = context.JobDetail.Where(x => x.Id == a.JobId).Select(x => x.JobName).FirstOrDefault(),//a.JobName,
                                    JobId = a.JobId,
                                    Remarks = a.Remarks,
                                    EstimateNumber = a.EstimateNumber,
                                    RefNumber = a.RefNumber,
                                    YourRef = a.YourRef,
                                    Status = a.Status,
                                    AccountId = a.AccountId,
                                    Asf = a.Asf,
                                    AsfAmount = a.AsfAmount,
                                    GrandTotal = a.GrandTotal,
                                    MultiPceId = a.MultiPceId,
                                    PayDescription = a.PayDescription,
                                    PoDocFileId = a.PoDocFileId,
                                    VatDocFileId = a.VatDocFileId,
                                    MainServiceCategory = a.MainServiceCategory,
                                    LegalId = a.LegalId,
                                    AffiliationId = a.AffiliationId,
                                    BusinessUnitId = a.BusinessUnitId,
                                    POClientNo = a.POClientNo,
                                    InvoiceBilled = a.InvoiceBilled,
                                    PceTypeCrossBilling = a.PceTypeCrossBilling,
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    JobStatus = c.JobStatusId,
                                    totalamtplusasf = (a.TotalAmount + a.AsfAmount),
                                    PceBalance = 0,
                                    ClientBrandName = g.BrandName,
                                    TermOfPayment = g.TermOfPayment,
                                    ClientInvAddress = g.BrandAddress,
                                    ClientPIC = g.ContactName,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == a.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<InvoiceClient>> GetPceList(string clientid, string legalid, string brandid, string mainservice, string currencyid, string divisionid, string invoiceId, bool? pceTypeCrossBilling)
        {
            var result = await (from b in context.Pces
                                join c in context.JobDetail on b.JobId equals c.Id
                                join d in context.LegalEntity on c.LegalEntityId equals d.Id
                                join f in context.BusinessUnits on c.BusinessUnitDivisionId equals f.Id
                                join g in context.CompanyBrands on c.BrandId equals g.Id
                                join h in context.Companys on g.CompanyId equals h.Id
                                //join i in context.Currencies on b.CurrencyId equals i.Id
                                join j in context.MainServiceCategorys on b.MainserviceCategoryId equals j.Id
                                where b.Status == StatusTransactionName.Approved
                                      && (clientid == "0" ? true : g.CompanyId == clientid)
                                      && (legalid == "0" ? true : c.LegalEntityId == legalid)
                                      && (brandid == "0" ? true : c.BrandId == brandid)
                                      && (mainservice == "0" ? true : b.MainserviceCategoryId == mainservice)
                                      && (currencyid == "0" ? true : (context.PceTasks.Where(x => x.PceId == b.Id && !string.IsNullOrEmpty(x.CurrencyId)).Select(x => x.CurrencyId).FirstOrDefault() ?? b.CurrencyId) == currencyid)
                                      && (divisionid == "0" ? true : c.BusinessUnitDivisionId == divisionid)
                                      && (pceTypeCrossBilling == null ? true : (pceTypeCrossBilling == true ? b.TypeOfExpenseId == TypeOfExpenseCrossBilling.TypeOfExpenseId : b.TypeOfExpenseId != TypeOfExpenseCrossBilling.TypeOfExpenseId))
                                      //&& b.PceReference == null
                                      && b.TotalBalanceInvoice > 0
                                      && !context.InvoiceClients.Any(x => x.PceId == b.Id && x.Status != InvoiceClientStatus.Approved && x.MultiPceId != null && (invoiceId == "0" ? true : x.MultiPceId != invoiceId))
                                select new InvoiceClient
                                {
                                    PceId = b.Id,
                                    Asf = b.OtherFeePercentage,
                                    AsfAmount = b.OtherFeeOriginal ?? b.OtherFee,
                                    //VAtPercentage = Math.Ceiling((decimal)((b.Vat / (b.Subtotal + b.OtherFee)) * 100)),
                                    //Vat = b.Vat,
                                    TotalAmount = b.SubtotalOriginal ?? b.Subtotal,
                                    GrandTotal = (b.SubtotalOriginal ?? b.Subtotal) + (b.OtherFeeOriginal ?? b.OtherFee),//b.Total, //khusus untuk multiple pce data pce yg di ambil tidak termasuk vat
                                    ClientName = h.CompanyName,
                                    ClientBrandName = g.BrandName,
                                    Status = InvoiceClientStatus.Open,
                                    InvoiceBilled = b.TotalUsedInvoice,
                                    PceBalance = b.TotalBalanceInvoice,
                                    JobId = c.Id,
                                    JobName = c.JobName,
                                    JobStatus = c.JobStatusId,
                                    LegalId = d.Id,
                                    LegalEntityName = d.LegalEntityName,
                                    BusinessUnitId = f.Id,
                                    DivisionName = f.UnitName,
                                    ClientId = h.Id,
                                    BrandId = g.Id,
                                    CurrencyId = context.PceTasks.Where(x => x.PceId == b.Id && !string.IsNullOrEmpty(x.CurrencyId)).Select(x => x.CurrencyId).FirstOrDefault() ?? b.CurrencyId,
                                    CurrencyName = context.Currencies.Where(x => x.Id == (context.PceTasks.Where(y => y.PceId == b.Id && !string.IsNullOrEmpty(y.CurrencyId)).Select(y => y.CurrencyId).FirstOrDefault() ?? b.CurrencyId)).Select(x => x.CurrencyName).FirstOrDefault(),
                                    MainServiceCategory = b.MainserviceCategoryId,
                                    ExchangeRate = context.PceTasks.Where(x => x.PceId == b.Id && !string.IsNullOrEmpty(x.CurrencyId)).Select(x => x.ExchangeRate).FirstOrDefault() ?? Convert.ToDecimal(b.CurrencyId == CurrencyIDR.CurrencyId ? "1" : "0"),
                                    TermOfPayment = g.TermOfPayment,
                                    POClientNo = b.PoClient,
                                    ClientInvAddress = g.BrandAddress,
                                    ClientTaxNumber = h.TaxRegistrationNumber,
                                    ClientPIC = g.ContactName,
                                    AffiliationId = c.AffiliationId,
                                    CreatedOn = b.CreatedOn,
                                    MainServiceCategoryName = j.Name,
                                    PceTypeCrossBilling = b.TypeOfExpenseId == TypeOfExpenseCrossBilling.TypeOfExpenseId,
                                    CurrencySymbol = (from y in context.PceTasks
                                                      join z in context.Currencies on (y.CurrencyId ?? b.CurrencyId) equals z.Id
                                                      where y.PceId == b.Id
                                                      select z.CurrencySymbol).FirstOrDefault(),
                                    CurrencyIdDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    CurrencySymbolDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<List<Journal>> GetDataJournal(string Id)
        {
            var result = await (from a in context.InvoiceClients
                                join b in context.JournalTemps on a.MultiPceId equals b.ReferenceId
                                join c in context.ChartOfAccounts on b.AccountId equals c.Id
                                where a.MultiPceId == Id && !(a.IsDeleted ?? false)
                                group new { a, b, c } by new { a.MultiPceId, JournalId = b.Id, b.ReferenceId, coaId = c.Id, c.CodeRec, c.Name5, b.Debit, b.Credit, b.JobId, b.LineCode, b.DivisiId, b.LegalId, b.DefaultCurrencyId } into g
                                select new Journal
                                {
                                    Id = g.Key.JournalId,
                                    ReferenceId = g.Key.ReferenceId,
                                    AccountId = g.Key.coaId,
                                    CodeRec = g.Key.CodeRec,
                                    AccountName = g.Key.Name5,
                                    Description = String.Join(",<br/>", g.Select(x => x.b.Description).Distinct().ToArray()),
                                    JobIdName = (context.JobDetail.Where(x => x.Id == g.Key.JobId).Select(x => $"{x.Id}-{x.JobName}").FirstOrDefault()),
                                    DivisiName = String.Join(",<br/>", context.BusinessUnits.Where(x => g.Any(z => z.a.BusinessUnitId == x.Id)).Select(x => x.UnitName).Distinct().ToArray()),
                                    LegalEntityName = String.Join(",<br/>", context.LegalEntity.Where(x => g.Any(z => z.a.LegalId == x.Id)).Select(x => x.LegalEntityName).Distinct().ToArray()),
                                    Credit = g.Key.Credit,
                                    Debit = g.Key.Debit,
                                    LineCode = g.Key.LineCode,
                                    DivisiId = g.Key.DivisiId,
                                    LegalId = g.Key.LegalId,
                                    JobId = g.Key.JobId,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == g.Key.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        public async Task<List<InvoiceClientItemOther>> GetPceTask(string pceid)
        {
            var result = await (from a in context.PceTasks
                                join b in context.Pces on a.PceId equals b.Id
                                where a.PceId == pceid
                                select new InvoiceClientItemOther()
                                {
                                    Id = a.Id,
                                    TaskId = a.Id,
                                    TaskName = a.TaskName,
                                    Quantity = a.Quantity,
                                    UnitPrice = a.TotalOriginal != null ? (a.Operator == "*" ? (a.UnitPrice * a.ExchangeRate) : (a.UnitPrice / a.ExchangeRate)) : a.UnitPrice,
                                    Amount = ((a.TotalOriginal ?? a.Total) ?? 0),
                                    AmountBilled = a.TotalOriginal ?? a.Total,
                                    PceID = a.PceId,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == (a.CurrencyId ?? b.CurrencyId)).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<int> AddAsync(InvoiceClient entity)
        {
            return await AddEdit(entity, false, false);
        }

        public async Task<int> EditAsync(InvoiceClient entity)
        {
            return await AddEdit(entity, true, false);
        }

        public async Task<int> CreateJournalAsync(InvoiceClient entity)
        {
            await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.InvoiceDate), entity.LegalId, Models.Enum.FinancePeriodCode.AR);
            bool isEdit = false;
            if (!string.IsNullOrEmpty(entity.MultiPceId)) isEdit = true;
            return await AddEdit(entity, isEdit, true);
        }

        public async Task<int> Approve(InvoiceClient model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.MultiPceId, StatusTransaction.Approved, model.LogDescription);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.MultiPceId && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = InvoiceClientStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            model.Status = InvoiceClientStatus.Approved;
                            model.ModifiedBy = context.GetEmployeeId();
                            model.ModifiedOn = DateTime.Now;

                            //post data AR
                            AccountReceivable accountReceivable = new AccountReceivable()
                            {
                                AccountReceivableNumber = model.MultiPceId,
                                Id = model.MultiPceId,
                                Amount = model.GrandTotal,
                                BrandId = model.BrandId,
                                JobId = String.Join(",", context.InvoiceClients.AsNoTracking().Where(x => x.MultiPceId == model.MultiPceId).Select(x => x.JobId).ToArray()),
                                LegalEntityId = model.LegalId,
                                AffilitionId = model.AffiliationId,
                                Status = ARStatus.Open,
                                BankAccountDestination = model.AccountId,
                                ExchangeRate = model.ExchangeRate == null ? 0 : model.ExchangeRate,
                                CurrencyId = model.CurrencyId,
                                TransactionDate = model.InvoiceDate,
                                BusinessUnitId = model.BusinessUnitId,
                                VatNo = model.VatNo,
                                ClientId = model.ClientId
                            };
                            await context.PhoenixAddAsync(accountReceivable);

                            var sqlUpdateInvoice = $@"UPDATE fn.invoice_client 
                                                    SET modified_by='{model.ModifiedBy}', modified_on='{Convert.ToDateTime(model.ModifiedOn).ToString("yyyy/MM/dd")}',
                                                        approved_by='{model.ModifiedBy}', approved_on='{Convert.ToDateTime(model.ModifiedOn).ToString("yyyy/MM/dd")}',
                                                        status='{model.Status}' WHERE multi_pce_id='{model.MultiPceId}'";
                            await context.Database.ExecuteSqlCommandAsync(sqlUpdateInvoice);

                            ////khusus untuk untuk multiple pce nilai balance selalu 0 di karenakan hanya sekali invoice dan nilai yg di invoice pasti sama dg data pce
                            //await context.SaveChangesAsync();
                            //var pceList = await context.Pces.AsNoTracking().Where(x => context.InvoiceClients.Any(z => z.PceId == x.Id && z.MultiPceId == model.MultiPceId)).ToListAsync();
                            //foreach (var item in pceList)
                            //{
                            //    item.TotalUsedInvoice = item.SubtotalOriginal ?? item.Subtotal;
                            //    item.TotalBalanceInvoice = 0;
                            //    item.CurrentInvoice = item.Total;
                            //    context.Pces.Update(item);
                            //};

                            //var task = await context.PceTasks.AsNoTracking().Where(x => pceList.Any(z => z.Id == x.PceId)).ToListAsync();
                            //foreach (var item in task)
                            //{
                            //    item.TotalUsedInvoice = item.TotalOriginal?? item.Total;
                            //    item.TotalBalanceInvoice = 0;
                            //    context.PceTasks.Update(item);
                            //}
                            var sqlUpdatePce = $@"UPDATE a 
                                                SET a.total_used_invoice = ISNULL(a.subtotal_original, a.Subtotal),
	                                                a.total_balance_invoice= 0,
	                                                a.current_invoice= ISNULL(a.subtotal_original, a.Subtotal)
                                                FROM pm.pce a
                                                WHERE EXISTS(SELECT * FROM fn.invoice_client b WHERE a.id=b.pce_id AND b.multi_pce_id='{model.MultiPceId}')";
                            await context.Database.ExecuteSqlCommandAsync(sqlUpdatePce);

                            var sqlUpdatePceTask = $@"UPDATE c
                                                    SET c.total_used_invoice = ISNULL(c.total_original, c.Total),
	                                                    c.total_balance_invoice = 0 
                                                    FROM pm.pce a
                                                    INNER JOIN pm.pce_task c ON c.pce_id = a.id
                                                    WHERE EXISTS(SELECT* FROM fn.invoice_client b WHERE a.id= b.pce_id AND b.multi_pce_id= '{model.MultiPceId}')";
                            await context.Database.ExecuteSqlCommandAsync(sqlUpdatePceTask);

                            await log.AddAsync(new StatusLog() { TransactionId = model.MultiPceId, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
                        }
                    }

                    save = await context.SaveChangesAsync();
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

        public async Task<int> Reject(InvoiceClient model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.MultiPceId, StatusTransaction.Rejected, model.LogDescription);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.MultiPceId && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = InvoiceClientStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = InvoiceClientStatus.Rejected;
                            var sql = $"UPDATE fn.invoice_client SET " +
                                $"modified_by='{context.GetEmployeeId()}', modified_on='{DateTime.Now.ToString("yyyy/MM/dd")}'," +
                                $"approved_by='{context.GetEmployeeId()}', approved_on='{DateTime.Now.ToString("yyyy/MM/dd")}'," +
                                $"status='{model.Status}' WHERE multi_pce_id='{model.MultiPceId}'";
                            await context.Database.ExecuteSqlCommandAsync(sql);
                            await log.AddAsync(new StatusLog() { TransactionId = model.MultiPceId, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
                        }
                    }

                    save = await context.SaveChangesAsync();
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

        private async Task<int> AddEdit(InvoiceClient invoice, bool isEdit, bool isJournal)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var model = await ProcessUpload(invoice);

                    if (!string.IsNullOrEmpty(model.MultiPceId))
                    {
                        var additional = await context.InvoiceClients.AsNoTracking().Where(x => x.MultiPceId == model.MultiPceId).ToListAsync();
                        if (additional.Count > 0)
                        {
                            context.InvoiceClients.RemoveRange(additional.ToArray());
                        }

                        var additionalOther = await context.InvoiceClientItemOthers.AsNoTracking().Where(x => x.InvoiceId == model.MultiPceId).ToListAsync();
                        if (additionalOther.Count > 0)
                        {
                            context.InvoiceClientItemOthers.RemoveRange(additionalOther.ToArray());
                        }
                    }

                    if (!isEdit)
                    {
                        model.MultiPceId = await TransID.GetTransId(context, Code.InvoiceClientCode, DateTime.Today);
                        model.InvoiceNumber = model.MultiPceId;
                    }

                    model.ModifiedBy = context.GetEmployeeId();
                    model.ModifiedOn = DateTime.Now;

                    if (isEdit)
                    {
                        context.Database.ExecuteSqlCommand($"UPDATE fn.e_faktur_detail SET is_used=0, reff_number=NULL WHERE reff_number={model.InvoiceNumber}");
                        if (!string.IsNullOrEmpty(model.VatNo))
                        {
                            context.Database.ExecuteSqlCommand($"UPDATE fn.e_faktur_detail SET is_used=1, reff_number={model.InvoiceNumber} WHERE vat_no={model.VatNo}");
                        }
                    }

                    var brandId = string.Empty;
                    if (model.MultipleBrand != null)
                    {
                        brandId = string.Join(";", model.MultipleBrand.Select(x => x.BrandId).ToArray());
                    }

                    foreach (var item in model.PceListMultiple)
                    {
                        item.Id = Guid.NewGuid().ToString();
                        item.MultiPceId = model.MultiPceId;
                        item.ClientId = model.ClientId;
                        item.BrandId = brandId;
                        item.AffiliationId = model.AffiliationId;
                        item.MainServiceCategory = model.MainServiceCategory;
                        item.BusinessUnitId = model.BusinessUnitId;
                        item.LegalId = model.LegalId;
                        item.InvoiceNumber = model.MultiPceId;
                        item.InvoiceDate = model.InvoiceDate;
                        item.POClientNo = model.POClientNo;
                        item.VatNo = model.VatNo;
                        item.PoDocFileId = model.PoDocFileId;
                        item.VatDocFileId = model.VatDocFileId;
                        item.YourRef = model.YourRef;
                        item.RefNumber = model.RefNumber;
                        item.CurrencyId = model.CurrencyId;
                        item.ExchangeRate = model.ExchangeRate;
                        item.Remarks = model.Remarks;
                        item.VAtPercentage = model.VAtPercentage;
                        item.Vat = model.Vat;
                        item.AccountId = model.AccountId;
                        item.AccountBankNumber = model.AccountBankNumber;
                        item.PayDescription = model.PayDescription;
                        item.Status = model.Status;
                        item.PceTypeCrossBilling = model.PceTypeCrossBilling;
                        item.CreatedBy = model.ModifiedBy;
                        item.CreatedOn = model.ModifiedOn;
                        item.ModifiedBy = model.ModifiedBy;
                        item.ModifiedOn = model.ModifiedOn;
                        item.PceSeq = 1;
                        item.VatExchangeRate = model.VatExchangeRate;
                        await context.AddAsync(item);
                    }

                    foreach (var item in model.InvoiceMultipleListOther)
                    {
                        var pce = model.PceListMultiple.Where(a => a.PceId == item.PceID).FirstOrDefault();
                        item.Id = Guid.NewGuid().ToString();
                        item.InvoiceId = model.MultiPceId;
                        item.JobID = pce.JobId;
                        item.Sequence = 1;
                        await context.AddAsync(item);
                    }

                    await context.SaveChangesAsync();
                    if (isJournal)
                    {
                        await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_ar_multi2_temp @p_account_receivable_id", parameters: new[] { new SqlParameter("@p_account_receivable_id", model.MultiPceId) });
                    }

                    var isApprove = true;
                    if (model.Status != InvoiceClientStatus.Open)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.InvoiceClientMultiplePce).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = model.MultiPceId,
                            DetailLink = $"{ApprovalLink.InvoiceClientMultiplePce}?Id={model.MultiPceId}",
                            NotificationType = NotificationTypeCode.InvoiceClientMultiple,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            if (!isJournal)
                            {
                                await log.AddAsync(new StatusLog() { TransactionId = model.MultiPceId, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
                            }
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        if (!isJournal)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = model.MultiPceId, Status = InvoiceClientStatus.StatusName(model.Status), Description = model.LogDescription });
                        }
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        private async Task<InvoiceClient> ProcessUpload(InvoiceClient entity)
        {
            var file1 = await uploadFile.Upload(entity.VatDocFile, null);
            if (!string.IsNullOrEmpty(file1))
            {
                entity.VatDocFileId = file1;
            }

            var file2 = await uploadFile.Upload(entity.PoDocFile, null);
            if (!string.IsNullOrEmpty(file2))
            {
                entity.PoDocFileId = file2;
            }
            return entity;
        }

        public async Task<int> UpdateJournalAsync(Journal entity)
        {
            context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET Description={entity.Description} WHERE id={entity.Id}");
            return await context.SaveChangesAsync();
        }

        public async Task<List<MultipleBrand>> GetMultipleBrandList(string multipceid)
        {
            var brandId = await context.InvoiceClients.Where(z => z.MultiPceId == multipceid && z.MultiPceId != null).Select(x => x.BrandId).FirstOrDefaultAsync() ?? "";

            var result = await context.CompanyBrands.Where(x => brandId.Split(";", StringSplitOptions.None).Any(z => z.Trim() == x.Id))
                .Select(x => new MultipleBrand
                {
                    BrandId = x.Id,
                    ClientBrandName = x.BrandName,
                    ClientInvAddress = x.BrandAddress,
                    ClientPIC = x.ContactName,
                    TermOfPayment = x.TermOfPayment
                }).ToListAsync();
            return result;
        }
    }
}
