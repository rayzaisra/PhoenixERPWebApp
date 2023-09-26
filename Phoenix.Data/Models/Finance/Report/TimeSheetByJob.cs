using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Finance.Report
{
   public class TimeSheetByJob
    {
        public string UserId { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionName { get; set; }
        public string JobName { get; set; }
       // public string job_id { get; set; }
        //public string client_id { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TotalHours { get; set; }
        public int TotalMinutes { get; set; }
        public string BusinessUnit { get; set; }
        public string LegalEntityName { get; set; }
    }
}

