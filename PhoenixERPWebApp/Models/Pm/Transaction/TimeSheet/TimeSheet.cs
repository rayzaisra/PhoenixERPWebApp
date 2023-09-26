using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Pm.Transaction.TimeSheet
{
    public class TimeSheet
    {
        public string id { get; set; }
        public string job_id { get; set; }
        public string company_name { get; set; }
        public string job_name { get; set; }
        public string non_job_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
    }
}
