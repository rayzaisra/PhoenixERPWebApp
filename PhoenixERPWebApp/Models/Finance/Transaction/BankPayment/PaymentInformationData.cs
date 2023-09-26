using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.BankPayment
{
    public class PaymentInformationData : FinanceEntityBase
    {
        public string Id { get; set; }
        public string EvidenceNo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Vat { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? TaxPayable { get; set; }
        public decimal? TaxPayablePercent { get; set; }
        public decimal? Charges { get; set; }
        public decimal ? Paid { get; set; }
        public decimal? Billing { get; set; }
        public bool? Recon { get; set; }
    }
}
