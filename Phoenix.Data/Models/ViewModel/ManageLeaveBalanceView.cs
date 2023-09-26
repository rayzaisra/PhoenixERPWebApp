using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Phoenix.Data.Models
{
    public class ManageLeaveBalanceView
    {
        [Key]
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string Grade { get; set; }
        public DateTime? JoinDate { get; set; }
        public string Status { get; set; }
        public int AnnualLeave { get; set; }
        public int CompensatoryLeave { get; set; }
        public int AnnualLeavePreviousYear { get; set; }
        //public int? MOnth { get; set; }
        public int Year { get; set; }
    }

    public class LeaveBalanceViewModel
    {
        public string EmployeeId { get; set; }
        public string LeaveBalance { get; set; }
        public string ExpiredDate { get; set; }
    }

}
