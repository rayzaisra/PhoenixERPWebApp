using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrConditionUserApproval
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Group Condition Index")]
        public int? GroupConditionIndex { get; set; }

        [DisplayName("Group Condition Approval")]
        public string GroupConditionApproval { get; set; }

        [DisplayName("Conditional Approval")]
        public string ConditionalApproval { get; set; }

        [DisplayName("Tr User Approval Id")]
        public string TrUserApprovalId { get; set; }

        [DisplayName("Status Approved")]
        public int? StatusApproved { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("Employee")]
        public string EmployeeName { get; set; }
    }
}