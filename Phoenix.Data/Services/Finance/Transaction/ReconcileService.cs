using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Models;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Enum;

namespace Phoenix.Data.Services.Finance.Transaction
{
    public interface IReconcileService
    {
        Task<List<ReconcileList>> GetList(ReconcileSearch search);
        Task<Reconcile> Get(string id);
        Task<int> AddAsync(Reconcile entity);
    }
    public class ReconcileService : IReconcileService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        readonly Services.Um.IManageMenuService menuService;
        readonly FileService uploadFile;
        readonly IPurchaseRequestService prService;
        readonly StatusLogService log;

        /// <summary>
        /// And endpoint to manage RequestForQuotation
        /// </summary>
        /// <param name="context">Database context</param>
        public ReconcileService(DataContext context, GlobalFunctionApproval globalFcApproval, Services.Um.IManageMenuService menuService, FileService uploadFile, IPurchaseRequestService prService, StatusLogService log)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.menuService = menuService;
            this.uploadFile = uploadFile;
            this.prService = prService;
            this.log = log;
        }

        public async Task<List<ReconcileList>> GetList(ReconcileSearch search)
        {
            if (search.Type == "CV")
            {
                var webServerUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value);
                var result = await (from tb in context.PaymentRequest
                                    join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                    join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                    where !(tb.IsDeleted ?? false) && tb.Status == PaymentRequestStatus.Complete
                                          //&& (string.IsNullOrEmpty(search.LegalEntityId) ? true : tb.LegalEntityId == search.LegalEntityId)
                                          && tb.LegalEntityId == search.LegalEntityId
                                          && (string.IsNullOrEmpty(search.AccountId) ? true : tb.AccountId == search.AccountId)
                                    select new ReconcileList
                                    {
                                        Id = tb.Id,
                                        BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                        PaymentTypeName = context.PaymentType.Where(x => x.Id == tb.PaymentTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                        ReconcileId = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Id).FirstOrDefault(),
                                        Status = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Count() < 1 ? ReconcileStatus.Unreconciled : context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Status).FirstOrDefault(),//tb.Status,
                                        AccountName = coa.Name5,
                                        AccountNo = coa.CodeRec,
                                        LegalEntityName = le.LegalEntityName,
                                        PaymentRequestDate = tb.PaymentRequestDate,
                                        PaymentRequestNumber = tb.PaymentRequestNumber,
                                        ReferenceNumberId = tb.ReferenceNumberId,
                                        //!string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                        //tb.SourceDoc == SourceDoc.CashAdvance ? $"<a href='{webServerUrl}/Finance/CashAdvance/Detail?Id={tb.ReferenceNumberId}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{tb.ReferenceNumberId}</span></a>" :
                                        //tb.SourceDoc == SourceDoc.PettyCash ? $"<a href='{webServerUrl}/Finance/PettyCash/Detail?Id={tb.ReferenceNumberId}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{tb.ReferenceNumberId}</span></a>" :
                                        //tb.SourceDoc == SourceDoc.AccountPayable ? string.Join(",", context.AccountPayables.Where(x => (tb.ReferenceNumberId ?? "").Contains(x.Id))
                                        //.Select(x => $"<a href='{webServerUrl}/Finance/InvoiceReceivable/Detail?name=ap&id={x.InvoiceReceivableId}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{x.Id}</span></a>").ToArray())
                                        //: tb.ReferenceNumberId
                                        //: "",
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
                                                     context.Companys.Where(x => x.Id == tb.PayToVendor).Select(x => x.CompanyName).FirstOrDefault(),
                                        //Period = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Period).FirstOrDefault(),
                                        //Notes = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Notes).FirstOrDefault(),
                                    }).OrderByDescending(x => x.PaymentRequestNumber).ToListAsync();
                return result;
            }
            else
            {
                var webServerUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value);
                var result = await (from tb in context.BankReceived
                                    join bu in context.BusinessUnits on tb.BusinessUnitId equals bu.Id
                                    join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                    join coa in context.ChartOfAccounts on tb.AccountId equals coa.Id
                                    where !(tb.IsDeleted ?? false)
                                          //&& (string.IsNullOrEmpty(search.LegalEntityId) ? true : tb.LegalEntityId == search.LegalEntityId)
                                          && tb.LegalEntityId == search.LegalEntityId
                                          && (string.IsNullOrEmpty(search.AccountId) ? true : tb.AccountId == search.AccountId)
                                    select new ReconcileList
                                    {
                                        Id = tb.Id,
                                        BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                        PaymentTypeName = context.PaymentType.Where(x => x.Id == tb.PaymentTypeId).Select(x => x.TypeName).FirstOrDefault(),
                                        ReconcileId = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Id).FirstOrDefault(),
                                        Status = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Count() < 1 ? ReconcileStatus.Unreconciled : context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Status).FirstOrDefault(),//tb.Status,
                                        AccountName = coa.Name5,
                                        AccountNo = coa.CodeRec,
                                        LegalEntityName = le.LegalEntityName,
                                        PaymentRequestDate = tb.PaymentRequestDate,
                                        PaymentRequestNumber = tb.PaymentRequestNumber,
                                        ReferenceNumberId = tb.ReferenceNumberId,
                                        //!string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                        //  string.Join(",", context.AccountReceivables.Where(x => (tb.ReferenceNumberId ?? "").Contains(x.Id))
                                        //  .Select(x => $"<a href='{webServerUrl}/Finance/AccountReceivable/Detail?id={x.Id}' title='View' class='edit'><span style='color:blue; text-decoration:underline; word-wrap: break-word'>{x.Id}</span></a>").ToArray())
                                        //  : "",
                                        AmountPay = tb.AmountPay,
                                        JobName = tb.JobId == null ? tb.PurchaseRequestId : string.Join(",<br/>", context.JobDetail.Where(x => tb.JobId.Contains(x.Id)).Select(x => $"{x.Id}-{x.JobName}").ToArray()),
                                        CurrencyName = string.IsNullOrEmpty(tb.ReferenceNumberId) ?
                                                      context.Currencies.Where(x => x.Id == tb.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault() :
                                                      (from a in context.AccountReceivables
                                                       join b in context.Currencies on a.CurrencyId equals b.Id
                                                       where tb.ReferenceNumberId.Contains(a.Id)
                                                       select b.CurrencyName).FirstOrDefault(),
                                        VendorName = context.Companys.Where(x => x.Id == tb.PayToVendor).Select(x => x.CompanyName).FirstOrDefault(),
                                        //Period = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Period).FirstOrDefault(),
                                        //Notes = context.Reconcile.Where(x => x.ReferenceId == tb.Id).Select(x => x.Notes).FirstOrDefault(),
                                    }).ToListAsync();
                return result;
            }
        }

        public async Task<Reconcile> Get(string id) => await context.Reconcile.Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();

        public async Task<int> AddAsync(Reconcile entity)
        {
            entity.Status = ReconcileStatus.Reconciled;
            if (!string.IsNullOrEmpty(entity.Id))
            {
                context.PhoenixEdit(entity);
            }
            else
            {
                context.PhoenixAdd(entity);
            }
            return await context.SaveChangesAsync();
        }
    }
}
