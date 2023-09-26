using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.BankPayment
{
    public class BankPaymentDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        public string BankPaymentId { get; set; }
        
        public string AccountId { get; set; }
        
        public decimal? Debit { get; set; }
        
        public decimal? Credit { get; set; }
    }
}
