using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.Finance.Report
{
    public class ReportTimeSheetByBusinessUnit
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string division { get; set; }
        //public string business_unit_name { get; set; }
        //public string total_time { get; set; }
        public string Non_Job { get; set; }
        public string Interface { get; set; }
        public string Pathfinder { get; set; }
        public string Central { get; set; }
        public string Nava_Charges { get; set; }
        public string SKOR { get; set; }
        public string Integrated_Marketing_Communications { get; set; }
        public string Trade_Marketing { get; set; }
        public string Experential { get; set; }
        public string Marketing_Services { get; set; }
        public string Relationship_Marketing { get; set; }
    }

    public class ReportTimeSheetByBusinessUnitNew
    {
        public string UserId { get; set; }
        public string EmployeeName { get; set; }
        public string LegalEntityName { get; set; }
        public string DivisionName { get; set; }
        //public string business_unit_name { get; set; }
        //public string total_time { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TotalHours { get; set; }
        public string Bu1 { get; set; }
        public string Bu2 { get; set; }
        public string Bu3 { get; set; }
        public string Bu4 { get; set; }
        public string Bu5 { get; set; }
        public string Bu6 { get; set; }
        public string Bu7 { get; set; }
        public string Bu8 { get; set; }
        public string Bu9 { get; set; }
        public string Bu10 { get; set; }
        public string Bu11 { get; set; }
        public string Bu12 { get; set; }
        public string BuNonJob { get; set; }
        public string TotalPercentage { get; set; }
        public string JobPercentage { get; set; }
        public string NonJobPercentage { get; set; }
    }
}
