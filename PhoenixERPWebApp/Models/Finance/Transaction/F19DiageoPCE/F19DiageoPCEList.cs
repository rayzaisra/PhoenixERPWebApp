using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.F19DiageoPCE
{
    public class F19DiageoPCEList :FinanceEntityBase
    {
        public string Id { get; set; }
        public DateTime Month { get; set; }
        public string Nature { get; set; }
        public string PENumber { get; set; }
        public string Status { get; set; }
        public string Brand { get; set; }
        public string PIC { get; set; }
        public string BudgetOwner { get; set; }
        public string CoreActivityDescription { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public decimal ASF { get; set; }
        public decimal VAT { get; set; }
        public decimal GrandTotalIDR { get; set; }
        public decimal GrandTotalUSD { get; set; }
    }
}
