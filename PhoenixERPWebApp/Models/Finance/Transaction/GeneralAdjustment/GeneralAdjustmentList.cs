using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.GeneralAdjustment
{
    public class GeneralAdjustmentList : FinanceEntityBase
    {
        public string Id { get; set; }
        public string EVDNo { get; set; }
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Vendor { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string ReceiptFrom { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }
}
