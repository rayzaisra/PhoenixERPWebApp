using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.FinanceReport
{
    public class SearchReportTimeSheet
    {
        [DisplayName("Job Number")]
        public string JobId { get; set; }
        [DisplayName("Client Number")]
        public string ClientId { get;set; }
        [DisplayName("User Name")]
        public string UserId { get; set; }
        [DisplayName("Month")]
        public int? Month { get; set; }

        [DisplayName("Year")]
        public int? Year { get; set; }
    }


}
