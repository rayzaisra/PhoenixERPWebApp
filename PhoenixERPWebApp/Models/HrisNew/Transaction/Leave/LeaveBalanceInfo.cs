using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LeaveBalanceInfo : HrisEntityBase
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Type Leave")]
        public string LeaveType { get; set; }

        [DisplayName("Total")]
        public Decimal Balance { get; set; }

        [DisplayName("Expired Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpiredDate { get; set; }
    }

    public class ListTableLeaveBalance
    {
        [DisplayName("Type Leave")]
        public string LeaveType { get; set; }
        public string EmployeBasicInfoId { get; set; }
        [DisplayName("Expired Date Current Year")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDateCurrentYear { get; set; }
        [DisplayName("Balance Current Year")]
        public Decimal BalanceCurrentYear { get; set; }
        [DisplayName("Balance Carry Over")]
        public Decimal BalanceCarryOver { get; set; }
        [DisplayName("Expired Date Carry Over")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDateCarryOver { get; set; }
    }

    public class TableLeaveBalance
    {
        public List<ListTableLeaveBalance> ListTableLeaveBalance { get; set; }
        public UseCarryLeaveBalance UseCarryLeaveBalance { get; set; }
    }

    public class UseCarryLeaveBalance
    {
        public bool UseCurrentYear { get; set; }
        public bool UseCarryOver { get; set; }
    }
}