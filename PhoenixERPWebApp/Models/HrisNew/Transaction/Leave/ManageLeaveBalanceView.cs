using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class ManageLeaveBalanceView : HrisEntityBase
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
        public int AnnualLeave { get; set; }

        [DisplayName("Compensatory Leave")]
        public int CompensatoryLeave { get; set; }

        [DisplayName("Annual Leave Previous Year")]
        public int AnnualLeavePreviousYear { get; set; }

        //[DisplayName("Month")]
        //public int? Month { get; set; }

        [DisplayName("Year")]
        public int Year { get; set; }
    }

    public class LeaveBalanceViewModel {
        public string EmployeeId { get; set; }
        public string LeaveBalance { get; set; }
        public string ExpiredDate { get; set; }
    }

}
