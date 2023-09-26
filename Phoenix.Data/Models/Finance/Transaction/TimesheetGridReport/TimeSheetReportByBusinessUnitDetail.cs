using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("timesheet_report_by_business_unit_detail", Schema = "fn")]
    public class TimeSheetReportByBusinessUnitDetail : FinanceEntityBase
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Column("timesheet_report_by_business_unit_id")]
        public string TimesheetReportByBusinessUnitId { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("total_time")]
        public string TotalTime { get; set; }
        [Column("user_id")]
        public string UserId { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("legal_entity_name")]
        public string LegalEntityName { get; set; }
        [Column("division_name")]
        public string DivisionName { get; set; }
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        [Column("total_hours")]
        public string TotalHours { get; set; }
        [Column("bu_1")]
        public string Bu1 { get; set; }
        [Column("bu_2")]
        public string Bu2 { get; set; }
        [Column("bu_3")]
        public string Bu3 { get; set; }
        [Column("bu_4")]
        public string Bu4 { get; set; }
        [Column("bu_5")]
        public string Bu5 { get; set; }
        [Column("bu_6")]
        public string Bu6 { get; set; }
        [Column("bu_7")]
        public string Bu7 { get; set; }
        [Column("bu_8")]
        public string Bu8 { get; set; }
        [Column("bu_9")]
        public string Bu9 { get; set; }
        [Column("bu_10")]
        public string Bu10 { get; set; }
        [Column("bu_11")]
        public string Bu11 { get; set; }
        [Column("bu_12")]
        public string Bu12 { get; set; }
        [Column("bu_non_job")]
        public string BuNonJob { get; set; }
        [Column("total_percentage")]
        public string TotalPercentage { get; set; }
        [Column("job_percentage")]
        public string JobPercentage { get; set; }
        [Column("non_job_percentage")]
        public string NonJobPercentage { get; set; }
    }

}

