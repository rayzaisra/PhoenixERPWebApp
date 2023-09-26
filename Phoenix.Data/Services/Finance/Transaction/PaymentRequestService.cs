using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Attributes;
using Phoenix.Data.Services.Um;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Finance.Transaction;
using System.Data.SqlClient;
using Phoenix.Data.Models.Um;
using Phoenix.Shared.Core.PrincipalHelpers;
using Phoenix.Data.Models.Finance.MasterData;
using Phoenix.Data.Models.Enum;

namespace Phoenix.Data.Services.Finance
{
    public interface IPaymentRequestService
    {
        Task<List<BankPaymentList>> GetPaymentRequestList(FilterPaymentRequest search);
        Task<List<AccountPayable>> GetListAPPosted(PaymentRequestFilter Filter);
        Task<List<AccountPayable>> GetListDetailAP(string Id);
        Task<List<AccountPayable>> GetListDetailPC(string Id);
        Task<List<AccountPayable>> GetListDetailSPR(string Id);
        Task<List<Journal>> GetBankPaymentJournal(string BankPaymentId);
        Task<Bank> GetAccountNumberData(string Id);
        Task<Bank> GetVendorData(string Id);
        //Task<CashAdvance> GetCAByNumber(string caNumber);
        Task<List<PaymentRequestDetail>> GetDetail(string Id);
        Task<PaymentRequest> Get(string Id);
        Task<int> AddAsync(PaymentRequest entity);
        Task<int> EditAsync(PaymentRequest entity);
        Task<int> ApproveAsync(PaymentRequest entity);
        Task<int> RejectAsync(PaymentRequest entity);
        Task<PaymentRequest> Reconsiliation(PaymentRequest entity);
        Task<int> CreateJournalAsync(PaymentRequest entity);
        Task<int> UpdateJournalAsync(Journal entity);
        Task<List<PaymentRequestDetailJournal>> GetEntryJournalList(string Id);
    }

    public class PaymentRequestService : IPaymentRequestService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly IManageMenuService menuService;
        readonly StatusLogService log;
        readonly FinancePeriodService financePeriod;
        /// <summary>
        /// And endpoint to manage PettyCash
        /// </summary>
        /// <param name="context">Database context</param>
        public PaymentRequestService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageMenuService menuService, StatusLogService log, FinancePeriodService financePeriod)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.log = log;
            this.financePeriod = financePeriod;
        }

        public async Task<List<BankPaymentList>> GetPaymentRequestList(FilterPaymentRequest search)
        {
            var webServerUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value);
            var month = (search.EndingDate.HasValue ? search.EndingDate.Value.Month : 0);
            var year = (search.EndingDate.HasValue ? search.EndingDate.Value.Year : 0);

            var result = await (from tb in context.PaymentRequest
                                join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                where !(tb.IsDeleted ?? false)
                                && (string.IsNullOrEmpty(search.Status) ? (tb.Status == PaymentRequestStatus.Open || tb.Status == PaymentRequestStatus.WaitingApproval) : tb.Status == search.Status)
                                && (search.DateType == "Day" ? (search.EndingDate == null ? true : tb.PaymentRequestDate == search.EndingDate) :
                                    search.DateType == "Month" ? (search.EndingDate == null ? true : (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Year : 0) == year && (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Month : 0) == month) :
                                    search.DateType == "Year" ? (search.EndingDate == null ? true : (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Year : 0) == year) :
                                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : tb.PaymentRequestDate >= search.StartingDate && tb.PaymentRequestDate <= search.EndingDate) : true)
                                select new BankPaymentList
                                {
                                    Id = tb.Id,
                                    PaymentTypeName = context.PaymentType.Where(x => x.Id == tb.PaymentTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    Status = tb.Status,
                                    LegalEntityName = le.LegalEntityName,
                                    PaymentRequestDate = tb.PaymentRequestDate,
                                    PaymentRequestNumber = tb.PaymentRequestNumber,
                                    ReferenceNumberId = !string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                                            tb.SourceDoc == SourceDoc.CashAdvance ? $"<a href='{webServerUrl}/Finance/CashAdvance/Detail?Id={tb.ReferenceNumberId}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{tb.ReferenceNumberId}</span></a>" :
                                                            tb.SourceDoc == SourceDoc.PettyCash ? $"<a href='{webServerUrl}/Finance/PettyCash/Detail?Id={tb.ReferenceNumberId}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{tb.ReferenceNumberId}</span></a>" :
                                                            tb.SourceDoc == SourceDoc.AccountPayable ? string.Join(",", context.AccountPayables.Where(x => (tb.ReferenceNumberId ?? "").Contains(x.Id))
                                                            .Select(x => $"<a href='{webServerUrl}/Finance/InvoiceReceivable/Detail?name=ap&id={x.InvoiceReceivableId}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{x.Id}</span></a>").ToArray())
                                                            : tb.ReferenceNumberId
                                                            : "",
                                    AmountPay = tb.AmountPay,
                                    JobName = tb.JobId == null ? tb.PurchaseRequestId : string.Join(",<br/>", context.JobDetail.Where(x => tb.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray()),
                                    CurrencyName = string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                                 context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault() :
                                                 (tb.SourceDoc == SourceDoc.CashAdvance) ? (from a in context.CashAdvances
                                                                                            join b in context.Currencies on a.CurrencyId equals b.Id
                                                                                            where tb.ReferenceNumberId.Contains(a.Id)
                                                                                            select b.CurrencyName).FirstOrDefault() :
                                                 (tb.SourceDoc == SourceDoc.AccountPayable) ? (from a in context.AccountPayables
                                                                                               join b in context.Currencies on a.CurrencyId equals b.Id
                                                                                               where tb.ReferenceNumberId.Contains(a.Id)
                                                                                               select b.CurrencyName).FirstOrDefault() :
                                                context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault(),
                                    VendorName = (tb.SourceDoc == SourceDoc.PettyCash || tb.SourceDoc == SourceDoc.SalaryPaymentRequest) ? context.LegalEntity.Where(x => x.Id == tb.PayToVendor).Select(x => x.LegalEntityName).FirstOrDefault() :
                                                 (tb.SourceDoc == SourceDoc.CashAdvance) ? (from a in context.CashAdvances
                                                                                            join b in context.Companys on a.VendorId equals b.Id
                                                                                            where tb.ReferenceNumberId.Contains(a.Id)
                                                                                            select b.CompanyName).FirstOrDefault() :
                                                 context.Companys.Where(x => x.Id == tb.PayToVendor).Select(x => x.CompanyName).FirstOrDefault()
                                }).OrderByDescending(x => x.PaymentRequestNumber).ToListAsync();
            return result;
        }

        public async Task<List<AccountPayable>> GetListAPPosted(PaymentRequestFilter Filter)
        {
            //sumber data dari ca tidak boleh di tampilkan.
            var result = await (from tb in context.AccountPayables
                                join inv in context.InvoiceReceivables on tb.InvoiceReceivableId equals inv.Id
                                join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                join ve in context.Companys on tb.VendorId equals ve.Id
                                //join bank in context.Banks on ve.BankId equals bank.Id
                                join po in context.PurchaseOrders on inv.PoId equals po.Id
                                join ms in context.MainServiceCategorys on tb.MainserviceCategoryId equals ms.Id
                                where !(tb.IsDeleted ?? false) && tb.IsPayment == false && //string.IsNullOrEmpty(po.CashAdvanceId) && 
                                inv.InvoiceStatusId == InvoiceReceivedStatus.Posted &&
                                !context.PaymentRequest.Any(x => x.ReferenceNumberId.Contains(tb.AccountPayableNumber) && !(x.IsDeleted ?? false) && x.Status != PaymentRequestStatus.Complete && (string.IsNullOrEmpty(Filter.PaymentRequestId) ? true : x.Id != Filter.PaymentRequestId)) &&
                                (string.IsNullOrEmpty(Filter.FilterLegalEntity) ? true : tb.LegalEntityId == Filter.FilterLegalEntity) &&
                                (string.IsNullOrEmpty(Filter.FilterBusinessUnitId) ? true : tb.BusinessUnitId == Filter.FilterBusinessUnitId) &&
                                (string.IsNullOrEmpty(Filter.MainServiceCategory) ? true : tb.MainserviceCategoryId == Filter.MainServiceCategory) &&
                                (string.IsNullOrEmpty(Filter.VendorId) ? true : tb.VendorId == Filter.VendorId) &&
                                (string.IsNullOrEmpty(Filter.CurrencyId) ? true : tb.CurrencyId == Filter.CurrencyId) &&
                                (string.IsNullOrEmpty(Filter.FilterApNum) ? true : tb.AccountPayableNumber == Filter.FilterApNum)
                                select new AccountPayable
                                {
                                    Id = tb.Id,
                                    AccountPayableNumber = tb.AccountPayableNumber,
                                    TransactionDate = tb.TransactionDate,
                                    BusinessUnitId = tb.BusinessUnitId,
                                    LegalEntityId = tb.LegalEntityId,
                                    VendorId = tb.VendorId,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = le.LegalEntityName,
                                    VendorName = ve.CompanyName,
                                    JobId = tb.JobId,
                                    JobName = context.JobDetail.Where(x => x.Id == tb.JobId).Select(x => $"{tb.JobId} - {x.JobName}").FirstOrDefault(),
                                    AccountId = context.Journal.Where(x => x.ReferenceId == tb.Id && x.LineCode == "1").Select(x => x.AccountId).FirstOrDefault(),
                                    //(x.Credit ?? 0) > 0).Select(x => x.AccountId).FirstOrDefault(),
                                    VendorAccountNumber = ve.AccountNumber,
                                    InvoiceReceivableId = tb.InvoiceReceivableId,
                                    BankAccountDestination = context.Banks.Where(x => x.Id == ve.BankId).Select(x => x.BankName).FirstOrDefault(),
                                    SwiftCode = context.Banks.Where(x => x.Id == ve.BankId).Select(x => x.SwiftCode).FirstOrDefault(),
                                    TaxPayableCoaName = context.ChartOfAccounts.Where(x => x.Id == inv.TaxPayableCoaId).Select(x => x.CodeRec + " -" + x.Name5).FirstOrDefault(),
                                    TaxPayableCoaId = inv.TaxPayableCoaId,
                                    InvoiceReceivedDate = inv.DueDate,
                                    MainserviceCategoryId = ms.Id,
                                    MainServiceCategoryName = ms.Name,
                                    //CurrencyId = tb.CurrencyId,
                                    CurrencyId = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    //ExchangeRate = tb.ExchangeRate,
                                    ExchangeRate = 1,//karena ketika proses bank payment selalu mengikuti currency default.
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencyIdDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    CurrencySymbolDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    //Amount = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (context.RequestForQuotationTask.Where(x => x.RequestForQuotationId == po.RequestForQuotationId).Select(x => x.Operator).FirstOrDefault() == "*" ?
                                    //                (tb.Amount * tb.ExchangeRate) : (tb.Amount / tb.ExchangeRate)),
                                    //TaxPayable = context.RequestForQuotationTask.Where(x => x.RequestForQuotationId == po.RequestForQuotationId).Select(x => x.Operator).FirstOrDefault() == "*" ?
                                    //                (inv.TaxPayable * tb.ExchangeRate) : (inv.TaxPayable / tb.ExchangeRate),

                                    //Vat = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (context.RequestForQuotationTask.Where(x => x.RequestForQuotationId == po.RequestForQuotationId).Select(x => x.Operator).FirstOrDefault() == "*" ?
                                    //                (inv.ValueAddedTax * tb.ExchangeRate) : (inv.ValueAddedTax / tb.ExchangeRate)),

                                    Amount = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (tb.Amount * tb.ExchangeRate),
                                    TaxPayable = (inv.TaxPayable * tb.ExchangeRate),
                                    Vat = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (inv.ValueAddedTax * tb.ExchangeRate),
                                    AmountOriginal = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : tb.Amount,
                                    TaxPayableOriginal = inv.TaxPayable,
                                    VatOriginal = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : inv.ValueAddedTax
                                }).ToListAsync();
            return result;
        }

        public async Task<List<AccountPayable>> GetListDetailAP(string Id)
        {
            var result = await (from prd in context.PaymentRequestDetail
                                join tb in context.AccountPayables on prd.ReferenceId equals tb.Id
                                join inv in context.InvoiceReceivables on tb.InvoiceReceivableId equals inv.Id
                                join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                join ve in context.Companys on tb.VendorId equals ve.Id
                                //join bank in context.Banks on ve.BankId equals bank.Id
                                join po in context.PurchaseOrders on inv.PoId equals po.Id
                                join ms in context.MainServiceCategorys on tb.MainserviceCategoryId equals ms.Id
                                where !(prd.IsDeleted ?? false) && prd.PaymentRequestId == Id
                                select new AccountPayable
                                {
                                    Id = tb.Id,
                                    AccountPayableNumber = tb.AccountPayableNumber,
                                    TransactionDate = tb.TransactionDate,
                                    BusinessUnitId = tb.BusinessUnitId,
                                    LegalEntityId = tb.LegalEntityId,
                                    VendorId = tb.VendorId,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = le.LegalEntityName,
                                    VendorName = ve.CompanyName,
                                    JobId = tb.JobId,
                                    JobName = context.JobDetail.Where(x => x.Id == tb.JobId).Select(x => $"{tb.JobId} - {x.JobName}").FirstOrDefault(),
                                    AccountId = prd.AccountId,
                                    VendorAccountNumber = ve.AccountNumber,
                                    InvoiceReceivableId = tb.InvoiceReceivableId,
                                    BankAccountDestination = context.Banks.Where(x => x.Id == ve.BankId).Select(x => x.BankName).FirstOrDefault(),
                                    SwiftCode = context.Banks.Where(x => x.Id == ve.BankId).Select(x => x.SwiftCode).FirstOrDefault(),
                                    TaxPayableCoaName = context.ChartOfAccounts.Where(x => x.Id == inv.TaxPayableCoaId).Select(x => x.CodeRec + " -" + x.Name5).FirstOrDefault(),
                                    TaxPayableCoaId = inv.TaxPayableCoaId,
                                    InvoiceReceivedDate = inv.DueDate,
                                    MainserviceCategoryId = ms.Id,
                                    MainServiceCategoryName = ms.Name,
                                    //CurrencyId = prd.CurrencyId,
                                    CurrencyId = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                                    //ExchangeRate = tb.ExchangeRate,
                                    ExchangeRate = 1,//karena ketika proses bank payment selalu mengikuti currency default.
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolDefault = context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    //Amount = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (context.RequestForQuotationTask.Where(x => x.RequestForQuotationId == po.RequestForQuotationId).Select(x => x.Operator).FirstOrDefault() == "*" ?
                                    //                (tb.Amount * tb.ExchangeRate) : (tb.Amount / tb.ExchangeRate)),
                                    //TaxPayable = context.RequestForQuotationTask.Where(x => x.RequestForQuotationId == po.RequestForQuotationId).Select(x => x.Operator).FirstOrDefault() == "*" ?
                                    //                (inv.TaxPayable * tb.ExchangeRate) : (inv.TaxPayable / tb.ExchangeRate),

                                    //Vat = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (context.RequestForQuotationTask.Where(x => x.RequestForQuotationId == po.RequestForQuotationId).Select(x => x.Operator).FirstOrDefault() == "*" ?
                                    //                (inv.ValueAddedTax * tb.ExchangeRate) : (inv.ValueAddedTax / tb.ExchangeRate)),
                                    Amount = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (tb.Amount * tb.ExchangeRate),
                                    TaxPayable = (inv.TaxPayable * tb.ExchangeRate),
                                    Vat = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : (inv.ValueAddedTax * tb.ExchangeRate),
                                    AmountOriginal = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : tb.Amount,
                                    TaxPayableOriginal = inv.TaxPayable,
                                    VatOriginal = !string.IsNullOrEmpty(po.CashAdvanceId) ? 0 : inv.ValueAddedTax
                                }).ToListAsync();
            return result;
        }

        public async Task<List<AccountPayable>> GetListDetailPC(string Id)
        {
            var result = await (from prd in context.PaymentRequestDetail
                                join tb in context.PettyCashs on prd.ReferenceId equals tb.Id
                                join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                where !(prd.IsDeleted ?? false) && prd.PaymentRequestId == Id
                                select new AccountPayable
                                {
                                    Id = tb.Id,
                                    AccountPayableNumber = tb.Id,
                                    TransactionDate = tb.SettlementDate,
                                    LegalEntityId = tb.LegalEntityId,
                                    Amount = prd.PayUnitPrice,
                                    CurrencyId = prd.CurrencyId,
                                    LegalEntityName = le.LegalEntityName,
                                    AccountId = prd.AccountId,
                                    ExchangeRate = tb.ExchangeRate,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<AccountPayable>> GetListDetailSPR(string Id)
        {
            var result = await (from prd in context.PaymentRequestDetail
                                join tb in context.SalaryPaymentRequests on prd.ReferenceId equals tb.Id
                                join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                where !(prd.IsDeleted ?? false) && prd.PaymentRequestId == Id
                                select new AccountPayable
                                {
                                    Id = tb.Id,
                                    AccountPayableNumber = tb.Id,
                                    TransactionDate = tb.TransactionDate,
                                    LegalEntityId = tb.LegalEntityId,
                                    Amount = prd.PayUnitPrice,
                                    CurrencyId = prd.CurrencyId,
                                    LegalEntityName = le.LegalEntityName,
                                    AccountId = prd.AccountId,
                                    ExchangeRate = tb.ExchangeRate,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<List<Journal>> GetBankPaymentJournal(string BankPaymentId)
        {
            var result = await (from tb in context.JournalTemps
                                join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                where tb.ReferenceId == BankPaymentId
                                select new Journal
                                {
                                    Id = tb.Id,
                                    ReferenceId = tb.ReferenceId,
                                    AccountId = coa.Id,
                                    CodeRec = coa.CodeRec,
                                    AccountName = coa.Name5,
                                    JobId = tb.JobId,
                                    Description = tb.Description == null ? coa.Name5 : tb.Description,
                                    JobIdName = context.JobDetail.Where(x => x.Id == tb.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault(),
                                    DivisiId = tb.DivisiId,
                                    DivisiName = context.BusinessUnits.Where(x => x.Id == tb.DivisiId).Select(x => x.UnitName).FirstOrDefault(),
                                    LegalEntityName = context.LegalEntity.Where(x => x.Id == tb.LegalId).Select(x => x.LegalEntityName).FirstOrDefault(),
                                    Credit = tb.Credit,
                                    Debit = tb.Debit,
                                    OriginalAmount = tb.OriginalAmount,
                                    LineCode = tb.LineCode,
                                    LegalId = tb.LegalId,
                                    CurrencySymbol = context.Currencies.Where(x => x.Id == tb.DefaultCurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                                    CurrencySymbolOriginal = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                                }).OrderByDescending(x => x.Debit).ThenByDescending(x => x.Credit).ToListAsync();
            return result;
        }

        public async Task<PaymentRequest> Get(string Id)
        {
            return await (from tb in context.PaymentRequest
                          join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                          where !(tb.IsDeleted ?? false) && tb.Id == Id
                          select new PaymentRequest
                          {
                              Id = tb.Id,
                              InvoiceReceivableId = tb.InvoiceReceivableId,
                              InvoiceReceivableNumber = tb.InvoiceReceivableNumber,
                              AccountId = tb.AccountId,
                              AdvertisingTax = tb.AdvertisingTax,
                              AffilicationId = tb.AffilicationId,
                              BusinessUnitId = tb.BusinessUnitId,
                              Amount = tb.Amount,
                              BankName = tb.BankName,
                              CurrencyId = tb.CurrencyId,
                              Discount = tb.Discount,
                              DueDate = tb.DueDate,
                              BankId = tb.BankId,
                              MainServiceCategoryId = tb.MainServiceCategoryId,
                              SourceDoc = tb.SourceDoc,
                              PayToVendor = tb.PayToVendor,
                              AccountNumber = tb.AccountNumber,
                              ExchangeRate = tb.ExchangeRate,
                              LegalEntityId = tb.LegalEntityId,
                              LegalEntityName = le.LegalEntityName,
                              PaymentRequestDate = tb.PaymentRequestDate,
                              PaymentRequestNumber = tb.PaymentRequestNumber,
                              PaymentTypeId = tb.PaymentTypeId,
                              TaxPayable = tb.TaxPayable,
                              TaxPayablePercent = tb.TaxPayablePercent,
                              Status = tb.Status,
                              AmountPay = tb.AmountPay,
                              ReferenceNumberId = tb.ReferenceNumberId,
                              Vat = tb.Vat,
                              VatPercent = tb.VatPercent,
                              TotalAmount = tb.TotalAmount,
                              PaytoName = tb.PaytoName,
                              JobName = string.IsNullOrEmpty(tb.JobId) ? tb.PurchaseRequestId : context.JobDetail.Where(x => x.Id == tb.JobId).Select(x => $"{x.Id} - {x.JobName}").FirstOrDefault() ?? tb.JobId.Substring(0, tb.JobId.Length - 1).Replace(",", ",</br>"),
                              PairingGroupId = tb.PairingGroupId,
                              Balance = tb.Balance,
                              BankChargesCoaId = tb.BankChargesCoaId,
                              Charges = tb.Charges,
                              Remarks = tb.Remarks,
                              JobId = tb.JobId,
                              ApprovedBy = tb.ApprovedBy,
                              ApprovedOn = tb.ApprovedOn,
                              CreatedBy = tb.CreatedBy,
                              CreatedOn = tb.CreatedOn,
                              DeletedBy = tb.DeletedBy,
                              DeletedOn = tb.DeletedOn,
                              IsActive = tb.IsActive,
                              IsDefault = tb.IsDefault,
                              IsDeleted = tb.IsDeleted,
                              IsLocked = tb.IsLocked,
                              IsMultiple = tb.IsMultiple,
                              ModifiedBy = tb.ModifiedBy,
                              ModifiedOn = tb.ModifiedOn,
                              OwnerId = tb.OwnerId,
                              PaytoBank = tb.PaytoBank,
                              PaytoCoaId = tb.PaytoCoaId,
                              PurchaseRequestId = tb.PurchaseRequestId,
                              Recon = tb.Recon,
                              TaxPayCoaId = tb.TaxPayCoaId,
                              PayToAccountNumber = tb.PayToAccountNumber,
                              PayToPettyCash = tb.PayToPettyCash,
                              PayToType = tb.PayToType,
                              TotalAmountPay = tb.TotalAmountPay,
                              TaxPayableCoa = tb.TaxPayableCoa,
                              CurrencySymbol = context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault(),
                              CurrencyIdDefault = (context.JournalTemps.Where(x => x.ReferenceId == Id).Select(x => x.DefaultCurrencyId)).FirstOrDefault() ??
                                        context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.Id).FirstOrDefault(),
                              CurrencySymbolDefault = (from o in context.JournalTemps
                                                       join p in context.Currencies on o.DefaultCurrencyId equals p.Id
                                                       where o.ReferenceId == Id
                                                       select p.CurrencySymbol).FirstOrDefault() ??
                                            context.Currencies.Where(x => !(x.IsDeleted ?? false) && x.IsDefault == true).Select(x => x.CurrencySymbol).FirstOrDefault(),
                          }).FirstOrDefaultAsync();
        }

        public async Task<Bank> GetAccountNumberData(string Id)
        {
            return await (from bank in context.Banks
                          join leg in context.LegalEntity on bank.LegalEntityId equals leg.Id
                          where !(bank.IsDeleted ?? false) && bank.Id == Id
                          select new Bank
                          {
                              AccountNumber = bank.AccountNumber,
                              LegalEntityId = bank.LegalEntityId,
                              LegalEntityName = leg.LegalEntityName,
                              SwiftCode = bank.SwiftCode,
                              CoaId = bank.CoaId,
                              CurrencyId = bank.CurrencyId
                          }).FirstOrDefaultAsync();
        }


        public async Task<Bank> GetVendorData(string Id)
        {
            return await (from pet in context.Companys
                          join leg in context.Banks on pet.BankId equals leg.Id
                          where !(pet.IsDeleted ?? false) && pet.Id == Id
                          select new Bank
                          {
                              AccountNumber = pet.AccountNumber,
                              LegalEntityId = leg.LegalEntityId,
                              CoaId = leg.CoaId,
                              BankName = leg.BankName,
                              SwiftCode = leg.SwiftCode
                          }).FirstOrDefaultAsync();
        }

        //public async Task<CashAdvance> GetCAByNumber(string caNumber)
        //{
        //    var model = await context.CashAdvances.Where(x => !(x.IsDeleted ?? false) && x.CaNumber == caNumber).FirstOrDefaultAsync();
        //    if (!string.IsNullOrEmpty(model.Id))
        //    {
        //        var company = await context.Companys.Where(x => x.Id == model.VendorId).FirstOrDefaultAsync();
        //        if (company != null)
        //        {
        //            model.VendorName = company.CompanyName;
        //            model.PayeeBankId = company.BankName;
        //            model.PayeeAccountNumber = company.AccountNumber;
        //            model.PayeeAccountName = company.AccountName;
        //            model.VendorAddress = company.OfficeAddress;
        //            model.VendorPIC = company.ContactName;
        //            model.VendorTaxNumber = company.TaxRegistrationNumber;
        //        }
        //    }
        //    return model;
        //}

        public async Task<List<PaymentRequestDetail>> GetDetail(string Id)
        {
            return await context.PaymentRequestDetail.Where(x => x.IsDeleted.Equals(false) && x.PaymentRequestId == Id).ToListAsync();
        }

        public async Task<int> AddAsync(PaymentRequest entity)
        {
            return await AddEditAsync(entity, false, false);
        }

        public async Task<int> EditAsync(PaymentRequest entity)
        {
            return await AddEditAsync(entity, true, false);
        }

        private async Task<int> AddEditAsync(PaymentRequest entity, bool isEdit, bool isJournal)
        {
            if (isJournal)
            {
                await financePeriod.IsClosePeriod(Convert.ToDateTime(entity.PaymentRequestDate), entity.LegalEntityId, Models.Enum.FinancePeriodCode.BP);
            }

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    //dikasih validasi ini dikarenakan ada kasus di mana business unit id gk kesimpan
                    if (entity.MainServiceCategoryId != MainServiceCategoryOther.MainServiceCategoryId)
                    {
                        if (string.IsNullOrEmpty(entity.BusinessUnitId)) { throw new Exception("business unit cannot be empty."); }
                    }

                    if (entity.Details != null)
                    {
                        var apNumber = string.Join(",", entity.Details.Select(x => x.APNumber).ToArray());
                        var jobId = string.Join(",", entity.Details.Select(x => x.JobId).ToArray());
                        if (!string.IsNullOrEmpty(apNumber)) entity.ReferenceNumberId = apNumber;
                        if (!string.IsNullOrEmpty(jobId)) entity.JobId = jobId;

                        if (entity.Details.Count > 1)
                        {
                            entity.IsMultiple = true;
                        }
                        else
                        {
                            entity.IsMultiple = false;
                        }
                    }

                    if (string.IsNullOrEmpty(entity.Id))
                    {
                        var payId = await TransID.GetTransId(context, Code.PaymentRequest, DateTime.Today);
                        entity.TempHeaderId = entity.Id;
                        entity.Id = payId;
                        entity.PaymentRequestNumber = payId;
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        context.PhoenixEdit(entity);

                        var detailJournal = context.PaymentRequestDetailJournal.AsNoTracking().Where(x => x.PaymentRequestId == entity.Id).ToList();
                        if (detailJournal.Count > 0)
                        {
                            context.PaymentRequestDetailJournal.RemoveRange(detailJournal);
                        }
                    }

                    if (entity.JournalEntry != null)
                    {
                        foreach (var item in entity.JournalEntry)
                        {
                            if (string.IsNullOrEmpty(item.Id))
                            {
                                item.Id = Guid.NewGuid().ToString();
                            }

                            item.PaymentRequestId = entity.Id;
                            item.TransactionDate = entity.PaymentRequestDate;
                            await context.PhoenixAddAsync(item);
                        };
                    }

                    if (entity.Details != null)
                    {
                        if (entity.Details.Count > 0)
                        {
                            var delmod = await context.PaymentRequestDetail.AsNoTracking().Where(x => x.PaymentRequestId == entity.Id).ToListAsync();
                            if (delmod.Count > 0)
                            {
                                foreach (PaymentRequestDetail data in delmod)
                                {
                                    ////update data ap yang ispayment nya true
                                    //var apdata = await context.AccountPayables.AsNoTracking().Where(x => x.Id == data.ReferenceId && x.IsPayment == true).FirstOrDefaultAsync();
                                    //if (apdata != null)
                                    //{
                                    //    apdata.IsPayment = false;
                                    //    context.Update(apdata);
                                    //}
                                    context.Database.ExecuteSqlCommand($"UPDATE fn.account_payable SET is_payment=0 WHERE id={data.ReferenceId}");

                                    //remove existing data
                                    context.PaymentRequestDetail.Remove(data);
                                }
                            }

                            foreach (PaymentRequestDetail data in entity.Details)
                            {
                                data.Id = Guid.NewGuid().ToString();
                                data.PaymentRequestId = entity.Id;
                                data.PairingGroupId = entity.PairingGroupId;
                                data.AccountId = await context.Journal.AsNoTracking().Where(x => x.ReferenceId == data.ReferenceId && x.LineCode == "1").Select(x => x.AccountId).FirstOrDefaultAsync();
                                await context.PhoenixAddAsync(data);

                                context.Database.ExecuteSqlCommand($"UPDATE fn.account_payable SET is_payment=1 WHERE id={data.ReferenceId}");

                                //var apdata = await context.AccountPayables.AsNoTracking().Where(x => x.Id == data.ReferenceId).FirstOrDefaultAsync();
                                //if (apdata != null)
                                //{
                                //    apdata.IsPayment = true;
                                //    if (apdata.LegalEntityId != null) { entity.LegalEntityId = apdata.LegalEntityId; }
                                //    if (apdata.BusinessUnitId != null) { entity.BusinessUnitId = apdata.BusinessUnitId; }
                                //    context.Update(apdata);
                                //}
                            }
                        }
                    }

                    if (isJournal)
                    {
                        await context.SaveChangesAsync();
                        var result = await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_payment_request @p_payment_request_id", parameters: new[] { new SqlParameter("@p_payment_request_id", entity.Id) });
                    }

                    if (entity.Status == PaymentRequestStatus.WaitingApproval)
                    {
                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.PaymentRequest).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.PaymentRequest}?Id={entity.Id}&isApprove=true",
                            NotificationType = NotificationTypeCode.PaymentRequestNotification,
                            Module = NotificationModuleType.ModuleFinance
                        };
                        var subGroupId = ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        var isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);
                        if (isApprove)
                        {
                            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PaymentRequestStatus.GetStatus(entity.Status), Description = entity.LogDescription });
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
                        await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PaymentRequestStatus.GetStatus(entity.Status), Description = entity.LogDescription });
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

        public async Task<int> ApproveAsync(PaymentRequest model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Approved, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = PaymentRequestStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Approved)
                        {
                            model.Status = PaymentRequestStatus.Complete;
                            await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.store_data_jurnal @p_id", parameters: new[] { new SqlParameter("@p_id", model.Id) });

                            //Create CA Settlement
                            if (model.SourceDoc == SourceDoc.CashAdvance)
                            {
                                var modelca = await context.CashAdvances.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CaNumber == model.ReferenceNumberId).FirstOrDefaultAsync();
                                if (modelca != null)
                                {
                                    var result = new CASettlement();
                                    result.Id = await TransID.GetTransId(context, Code.CashAdvanceSattlement, DateTime.Today);
                                    result.SettlementNumber = result.Id;
                                    result.Status = StatusTransactionName.Draft;
                                    result.CaSettlementDate = DateTime.Now;
                                    result.CashAdvanceId = modelca.Id;
                                    result.PaymentRequestId = model.Id;
                                    result.JobId = modelca.JobId;
                                    result.PcaId = modelca.PcaId;
                                    result.MainserviceCategoryId = modelca.MainserviceCategoryId;
                                    result.BusinessUnitId = modelca.BusinessUnitId;
                                    result.LegalEntityId = modelca.LegalEntityId;
                                    result.AffiliationId = modelca.AffiliationId;
                                    result.VendorId = modelca.VendorId;
                                    result.VendorName = modelca.VendorName;
                                    result.PceTypeId = modelca.PceTypeId;
                                    result.CurrencyId = modelca.CurrencyId;
                                    result.ExchangeRate = modelca.ExchangeRate;
                                    result.RequestEmployeeId = modelca.RequestEmployeeId;
                                    result.RequestEmployeeName = modelca.RequestEmployeeName;
                                    result.RequestJobTitleId = modelca.RequestJobTitleId;
                                    result.RequestJobTitleName = modelca.RequestJobTitleName;
                                    result.RequestJobGradeId = modelca.RequestJobGradeId;
                                    result.RequestJobGradeName = modelca.RequestJobGradeName;
                                    result.RequestedBy = modelca.RequestedBy;
                                    result.RequestedOn = modelca.RequestedOn;
                                    result.OnBehalfId = modelca.OnBehalfId;
                                    result.Amount = 0;//context.CashAdvanceDetails.Where(x => x.CashAdvanceId == modelca.Id).Sum(x => x.TotalRequest);
                                    result.Vat = 0;
                                    result.VatPercent = 0;
                                    result.TotalAmount = 0;//result.Amount + result.Vat;
                                    result.CaBalance = 0;//modelca.Amount - result.TotalAmount;
                                    await context.PhoenixAddAsync(result);
                                    await context.SaveChangesAsync();

                                    var detail = await context.CashAdvanceDetails.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CashAdvanceId == modelca.Id)
                                        .Select(x => new CASettlementDetail
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            CaSettlementId = result.Id,
                                            CaDetailId = x.Id,
                                            TaskId = x.TaskId,
                                            TaskName = x.TaskName,
                                            UomId = x.UomId,
                                            Qty = x.Qty,
                                            //UnitPrice = (x.TotalRequest / x.Qty),
                                            //Amount = x.UnitPrice * x.Qty,
                                            TotalRequest = x.TotalRequest,
                                            TotalExchangeRequest = x.TotalExchange,
                                            //TotalExchangeAmount = x.TotalExchange
                                        }).ToListAsync();
                                    await context.PhoenixAddRangeAsync(detail.ToArray());

                                    foreach (var item in detail)
                                    {
                                        var pcaTask = await context.PcaTasks.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.PcaId == modelca.PcaId && x.Id == item.TaskId).FirstOrDefaultAsync();
                                        if (pcaTask == null) throw new Exception("PCA Task cannot be empty");
                                        pcaTask.Status = PCAStatus.CAS;
                                        //pcaTask.TotalPriceUsed = pcaTask.TotalPriceUsed + detail.TotalExchange;
                                        //pcaTask.TotalPriceBalance = pcaTask.TotalPrice - pcaTask.TotalPriceUsed;

                                        //if (pcaTask.TotalPriceBalance == null)
                                        //{
                                        //    throw new Exception("Remaining Budget Cannot be Empty.");
                                        //}

                                        //if (pcaTask.TotalPriceBalance < 0)
                                        //{
                                        //    throw new Exception("the total exchange requests must be less than or equal to the remaining budget");
                                        //}

                                        context.PhoenixEdit(pcaTask);
                                    }
                                }
                            }

                            //if (model.PayToType == "1")//PETTYCASH
                            //{
                            //    var modelPettycah = await context.MasterPettyCashs.AsNoTracking().Where(x => x.IsDeleted.Equals(false) && x.Id == model.PayToPettyCash).FirstOrDefaultAsync();
                            //    modelPettycah.AmountInsused = (modelPettycah.AmountInsused == null ? 0 : modelPettycah.AmountInsused) + model.AmountPay;
                            //    modelPettycah.Balance = model.AmountPay;
                            //    context.PhoenixEdit(modelPettycah);
                            //}
                        }

                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = PaymentRequestStatus.GetStatus(model.Status), Description = model.LogDescription });
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

        public async Task<int> RejectAsync(PaymentRequest model)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await globalFcApproval.UnsafeProcessApproval(model.Id, StatusTransaction.Rejected, model.LogDescription, model);
                    var approval = context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == model.Id && !(x.IsDeleted ?? false)).FirstOrDefault();
                    if (approval != null)
                    {
                        model.Status = PaymentRequestStatus.WaitingApproval;
                        if (approval.StatusApproved == Models.Um.StatusApproved.Rejected)
                        {
                            model.Status = PaymentRequestStatus.Reject;
                        }

                        context.PhoenixEdit(model);
                        await log.AddAsync(new StatusLog() { TransactionId = model.Id, Status = PaymentRequestStatus.GetStatus(model.Status), Description = model.LogDescription });
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

        public async Task<PaymentRequest> Reconsiliation(PaymentRequest entity)
        {
            entity.Status = PaymentRequestStatus.Reconsiliation;
            context.PhoenixEdit(entity);
            await log.AddAsync(new StatusLog() { TransactionId = entity.Id, Status = PaymentRequestStatus.GetStatus(PaymentRequestStatus.Reconsiliation), Description = entity.LogDescription });
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CreateJournalAsync(PaymentRequest entity)
        {
            bool isEdit = false;
            if (!string.IsNullOrEmpty(entity.Id)) isEdit = true;
            return await AddEditAsync(entity, isEdit, true);

            //var result = await context.Database.ExecuteSqlCommandAsync("EXECUTE fn.create_jurnal_payment_request @p_payment_request_id", parameters: new[] { new SqlParameter("@p_payment_request_id", PaymentRequestId) });
            //var save = await context.SaveChangesAsync();
            //return save;
        }

        public async Task<int> UpdateJournalAsync(Journal entity)
        {
            context.Database.ExecuteSqlCommand($"UPDATE fn.journal_temp SET Description={entity.Description} WHERE id={entity.Id}");
            return await context.SaveChangesAsync();
        }

        public async Task<List<PaymentRequestDetailJournal>> GetEntryJournalList(string Id)
        {
            return await (from a in context.PaymentRequestDetailJournal
                          join b in context.ChartOfAccounts on a.CoaId equals b.Id
                          join c in context.PaymentRequest on a.PaymentRequestId equals c.Id
                          where a.PaymentRequestId == Id
                          select new PaymentRequestDetailJournal
                          {
                              Id = a.Id,
                              PaymentRequestId = a.PaymentRequestId,
                              CoaId = a.CoaId,
                              Description = a.Description,
                              Condition = a.Condition,
                              Debit = a.Debit,
                              Credit = a.Credit,
                              TransactionDate = a.TransactionDate,
                              //CodeRec = b.CodeRec,
                              AccountName = $"({b.CodeRec}) {b.Name5}",
                              CreatedBy = a.CreatedBy,
                              CreatedOn = a.CreatedOn,
                              CurrencySymbol = context.Currencies.Where(x => x.Id == c.CurrencyId).Select(x => x.CurrencySymbol).FirstOrDefault()
                          }).ToListAsync();
        }

    }
}
