using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.StatementOfAccountPayable
{
    public class StatementOfAccountPayableIndex 
    {
        public string ChartOfAccount { get; set; }
        public string Vendor { get; set; }
        public string Legal { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
