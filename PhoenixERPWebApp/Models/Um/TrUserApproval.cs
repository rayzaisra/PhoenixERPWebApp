using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrUserApproval
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Is Condition")]
        public bool? IsCondition { get; set; }

        [DisplayName("Tr Temp Approval Id")]
        public string TrTempApprovalId { get; set; }

        [DisplayName("Index User")]
        public int? IndexUser { get; set; }

        [DisplayName("Status Approved")]
        public int? StatusApproved { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("Approval")]
        public string EmployeeName { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        [DisplayName("Status")]
        public string StatusApprovedDescription { get; set; }

        public string Jobtitle { get; set; }
       
        public virtual List<TrConditionUserApproval> ListConditionalApprovals { get; set; }

        public int? RowNumber { get; set; }
    }
}