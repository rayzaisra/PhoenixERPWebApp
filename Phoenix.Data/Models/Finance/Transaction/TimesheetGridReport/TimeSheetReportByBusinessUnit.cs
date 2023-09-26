using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("timesheet_report_by_business_unit", Schema = "fn")]
    public class TimeSheetReportByBusinessUnit : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id")]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("month_period")]
        public string MonthPeriod { get; set; }

        [Column("year_period")]
        public string YearPeriod { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("company_id")]
        public string CompanyId { get; set; }
        [Column("job_id")]
        public string JobId { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string RequestName { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string JobName { get; set; }
        [NotMapped]
        public List<TimeSheetReportByBusinessUnitDetail> Detail { get; set; }

    }

    
}
