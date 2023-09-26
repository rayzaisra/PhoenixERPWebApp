using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Overtime), Schema = "hr")]
    public class Overtime : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [Column("overtime_date")]
        public DateTime OvertimeDate { get; set; }

        [Required]
        [Column("hour_overtime")]
        public TimeSpan HourOvertime { get; set; }

        [Column("reason_overtime"), StringLength(2147483647)]
        public string ReasonOvertime { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [MaxLength(50)]
        [Column("request_code"), StringLength(50)]
        public string RequestCode { get; set; }

        [MaxLength(50)]
        [Column("leave_balance_id"), StringLength(50)]
        public string LeaveBalanceId { get; set; }

        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public string RequestorName { get; set; }
    }

    public class OvertimeReport
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public string EmployeeName { get; set; }
        public string LegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public string JobTitleId { get; set; }
        public string JobTitleName { get; set; }
        public string JobGradeId { get; set; }
        public string JobGradeName { get; set; }
        public string TotalOvertime { get; set; }
        public int CDO { get; set; }
        public decimal? BalanceCDO { get; set; }
        public int Year { get; set; }
    }

    public class OvertimeReportBody
    {

        public string OvertimeId { get; set; }

        public DateTime? OvertimeDate { get; set; }

        public DateTime? HoursDuration { get; set; }

        public string HoursDurationString { get; set; }

        public string UserApprovalId { get; set; }

        public string UserApprovalName { get; set; }

        public DateTime? ApprovedDate { get; set; }
    }

    public class OvertimeReportDetail
    {
        public OvertimeReport Header { get; set; }
        public List<OvertimeReportBody> Detail { get; set; }
    }
}