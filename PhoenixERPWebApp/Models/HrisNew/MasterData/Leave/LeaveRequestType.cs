using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LeaveRequestType : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Is Reduce Balance")]
        public bool IsReduceBalance { get; set; }

        [DisplayName("Is Paid Leave")]
        public bool IsPaidLeave { get; set; }

        [DisplayName("Minimum Work")]
        public int MinimumWork { get; set; }

        [DisplayName("Minimum Duration Leave")]
        public int MinimumDurationLeave { get; set; }

        [DisplayName("Maximum Duration Leave")]
        public int MaximumDurationLeave { get; set; }

        [DisplayName("Leave Duration In A Year")]
        public int LeaveDurationInAYear { get; set; }

        [DisplayName("Is Carry Over")]
        public bool? IsCarryOver { get; set; }
        [DisplayName("Maximum Days In A Month")]
        public int MaximumDaysInAMonth { get; set; }
        [DisplayName("Employee Grade Level")]
        public string GradeLevelEmployeeId { get; set; }
        [DisplayName("Employee Grade Level")]
        public string GradeLevelEmployeeName { get; set; }
    }
}