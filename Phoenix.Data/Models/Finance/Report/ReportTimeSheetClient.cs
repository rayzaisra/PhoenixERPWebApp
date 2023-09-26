using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Finance.Report
{
   public class ReportTimeSheetClient
    {
        public string client_name { get; set; }
        public string name_employee { get; set; }
        public string job_number { get; set; }
        public string job_name { get; set; }
        public string business_unit_division_id { get; set; }
        public DateTime start_date_weekly { get; set; }
        public DateTime end_date_weekly { get; set; }
    }
}
