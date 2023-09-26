using Phoenix.Data.Models.Finance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Enum;

namespace Phoenix.Data.Services.Finance
{
    public interface IBankPaymentService
    {
        Task<List<BankPaymentList>> Get(FilterPaymentRequest search);
    }

    public class BankPaymentService : IBankPaymentService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage PettyCash
        /// </summary>
        /// <param name="context">Database context</param>
        public BankPaymentService(DataContext context)
        {
            this.context = context;
        }
        public async Task<List<BankPaymentList>> Get(FilterPaymentRequest search)
        {
            var webServerUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value);
            var month = (search.EndingDate.HasValue ? search.EndingDate.Value.Month : 0);
            var year = (search.EndingDate.HasValue ? search.EndingDate.Value.Year : 0);

            var result = await (from tb in context.PaymentRequest
                                join le in context.LegalEntity on tb.LegalEntityId equals le.Id
                                where !(tb.IsDeleted ?? false) && tb.Status == PaymentRequestStatus.Complete
                                && (search.DateType == "Day" ? (search.EndingDate == null ? true : tb.PaymentRequestDate == search.EndingDate) :
                                    search.DateType == "Month" ? (search.EndingDate == null ? true : (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Year : 0) == year && (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Month : 0) == month) :
                                    search.DateType == "Year" ? (search.EndingDate == null ? true : (tb.PaymentRequestDate.HasValue ? tb.PaymentRequestDate.Value.Year : 0) == year) :
                                    search.DateType == "Custom Range" ? (search.StartingDate == null && search.EndingDate == null ? true : tb.PaymentRequestDate >= search.StartingDate && tb.PaymentRequestDate <= search.EndingDate) : true)
                                select new BankPaymentList
                                {
                                    Id = tb.Id,
                                    BusinessUnitName = context.BusinessUnits.Where(x => x.Id == tb.BusinessUnitId).Select(x => x.UnitName).FirstOrDefault(),
                                    PaymentTypeName = context.PaymentType.Where(x => x.Id == tb.PaymentTypeId).Select(x => x.TypeName).FirstOrDefault(),
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
    }
}
