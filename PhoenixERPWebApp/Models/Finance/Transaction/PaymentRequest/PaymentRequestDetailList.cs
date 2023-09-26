using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.PaymentRequest
{
    public class PaymentRequestDetailList
    {
        public string Id { get; set; }
        public decimal EVDNo { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal Applied { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxation { get; set; }
        public decimal EndingBalance { get; set; }
    }
}
