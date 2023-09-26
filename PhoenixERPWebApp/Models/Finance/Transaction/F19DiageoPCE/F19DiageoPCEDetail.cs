using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.F19DiageoPCE
{
    public class F19DiageoPCEDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        public DateTime Year { get; set; }
        public DateTime Month { get; set; }
    }
}
