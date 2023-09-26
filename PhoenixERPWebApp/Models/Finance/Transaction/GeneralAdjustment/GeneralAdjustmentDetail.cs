using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.GeneralAdjustment
{
    public class GeneralAdjustmentDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string LegalEntity { get; set; }
        public string Status { get; set; }
        public string Voucher { get; set; }
        public string Charges { get; set; }
        public string Affiliate { get; set; }
        public string Description { get; set; }
        public string Exchange { get; set; }
        public string ShareServices { get; set; }
        public string Reference { get; set; }
        public DateTime DueDate { get; set; }
    }
}
