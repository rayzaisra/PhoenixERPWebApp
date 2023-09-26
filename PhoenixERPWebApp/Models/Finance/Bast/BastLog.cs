using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.BAST
{
    public class BastLog
    {
        public string NameEmployee { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
    }
}
