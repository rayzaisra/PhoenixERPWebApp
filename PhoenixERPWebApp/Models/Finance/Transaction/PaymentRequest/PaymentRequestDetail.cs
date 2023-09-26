using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.PaymentRequest
{
    public class PaymentRequestDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Ref { get; set; }
        public string RefNumber { get; set; }
        public string DepositTo { get; set; }
        public DateTime DueDate { get; set; }
        public string Voucher { get; set; }
        public string CC { get; set; }
        public decimal Amount { get; set; }
        public decimal Charges { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public string Vendor { get; set; }
        public string Account { get; set; }
        public decimal? Exchange { get; set; }
        public string Department { get; set; }
        public string Payment { get; set; }
        public string APNumber { get; set; }


        public string PaymentRequestId { get; set; }
        public string CurrencyId { get; set; }
        public decimal? Qty { get; set; }
        public decimal? PayUnitPrice { get; set; }
        public string ReferenceId { get; set; }
        public string AccountId { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public decimal? Vat { get; set; }
        public decimal? TaxPayable { get; set; }
        public string SouceDoc { get; set; }
        public string PairingGroupId { get; set; }
        public string TaxPayableCoaId { get; set; }
        public string TaxPayableCoaName { get; set; }
    }
}
