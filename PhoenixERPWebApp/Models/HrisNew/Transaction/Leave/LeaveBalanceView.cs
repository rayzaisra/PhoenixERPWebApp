using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LeaveBalanceView : HrisEntityBase
    {
        [Key]
        [DisplayName("Employee ID")]
        public string EmployeeId { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        [DisplayName("Grade")]
        public string Grade { get; set; }

        [DisplayName("Join Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoinDate { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Annual Leave")]
        public Decimal AnnualLeave { get; set; }

        [DisplayName("Compensatory Leave")]
        public Decimal CompensatoryLeave { get; set; }

        [DisplayName("Annual Leave Previous Year")]
        public Decimal AnnualLeavePreviousYear { get; set; }

        //[DisplayName("Month")]
        //public int? Month { get; set; }

        [DisplayName("Year")]
        public int Year { get; set; }
    }


    public class GenerateLeaveBalanceByLeaveRequestType
    {
        [Key]
        [DisplayName("Employee ID")]
        public string EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        [DisplayName("Job Title")]
        public string JobTitleName { get; set; }
        [DisplayName("Job Grade")]
        public string JobGradeName { get; set; }
        [DisplayName("Join Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? JoinDate { get; set; }
        [DisplayName("Employee Status")]
        public string EmploymentStatusName { get; set; }
        [DisplayName("Leave Type")]
        public string LeaveType { get; set; }
        [DisplayName("Balance Current Year")]
        public decimal? BalanceCurrentYear { get; set; }
        [DisplayName("Expired Date Current Year")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDateCurrentYear { get; set; }
        [DisplayName("Balance Carry Over")]
        public decimal? BalanceCarryOver { get; set; }
        [DisplayName("Expired Date Carry Over")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDateCarryOver { get; set; }
        [DisplayName("Total Compensatory Day Off")]
        public decimal? TotalCompensatoryDayOff { get; set; }
        [DisplayName("Expired Year")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredYear { get; set; }
    }
}