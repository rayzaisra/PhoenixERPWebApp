using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Overtime : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OvertimeDate { get; set; }

        [DisplayName("Duration")]
        public TimeSpan HourOvertime { get; set; }

        [DisplayName("Reason for Overtime Required")]
        public string ReasonOvertime { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Request Code")]
        public string RequestCode { get; set; }

        [DisplayName("Leave Balance Id")]
        public string LeaveBalanceId { get; set; }

        //[NotMapped]

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }


        public bool? IsApprove { get; set; }

        public InfoEmployee requestEmployee { get; set; }

        //[NotMapped]
        [DisplayName("Hour Overtime")]
        public string HourOver { get { return ((TimeSpan)HourOvertime).ToString(@"hh\:mm"); } }
        [DisplayName("Requestor Name")]
        public string RequestorName { get; set; }

    }

    public class OvertimeReport
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        [DisplayName("Job Title")]
        public string JobTitleId { get; set; }
        [DisplayName("Job Title")]
        public string JobTitleName { get; set; }
        [DisplayName("Job Grade")]
        public string JobGradeId { get; set; }
        [DisplayName("Job Grade")]
        public string JobGradeName { get; set; }
        [DisplayName("Total Overtime")]
        public string TotalOvertime { get; set; }
        [DisplayName("CDO")]
        public int CDO { get; set; }
        [DisplayName("Balance CDO")]
        public decimal? BalanceCDO { get; set; }
        [DisplayName("Year")]
        public int Year { get; set; }
    }

    public class OvertimeReportBody
    {
        [DisplayName("Overtime Id")]
        public string OvertimeId { get; set; }
        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OvertimeDate { get; set; }
        [DisplayName("Hours Duration")]
        public DateTime? HoursDuration { get; set; }
        [DisplayName("Hours Duration")]
        public string HoursDurationString { get; set; }
        [DisplayName("Approved By")]
        public string UserApprovalId { get; set; }
        [DisplayName("Approved Name")]
        public string UserApprovalName { get; set; }
        [DisplayName("Approved Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedDate { get; set; }
    }

    public class OvertimeReportDetail
    {
        public OvertimeReport Header { get; set; }
        public List<OvertimeReportBody> Detail { get; set; }
    }
}