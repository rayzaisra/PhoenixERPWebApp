using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrTemplateApproval
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Ref Id")]
        public string RefId { get; set; }

        [DisplayName("Template Name")]
        public string TemplateName { get; set; }

        [DisplayName("Sub Group Id")]
        public string SubGroupId { get; set; }

        [DisplayName("Division Id")]
        public string DivisionId { get; set; }

        [DisplayName("Is Notify By Email")]
        public bool? IsNotifyByEmail { get; set; }

        [DisplayName("Is Notify By Web")]
        public bool? IsNotifyByWeb { get; set; }

        [DisplayName("Due Date")]
        public int? DueDate { get; set; }

        [DisplayName("Reminder")]
        public int? Reminder { get; set; }

        [DisplayName("Approval Category")]
        public int? ApprovalCategory { get; set; }

        [DisplayName("Approval Type")]
        public int? ApprovalType { get; set; }

        [DisplayName("Status Approved")]
        public int? StatusApproved { get; set; }

        [DisplayName("Remark Rejected")]
        public string RemarkRejected { get; set; }

        [DisplayName("Detail Link")]
        public string DetailLink { get; set; }

        [DisplayName("Form Req Name")]
        public string FormReqName { get; set; }

        [DisplayName("Rejected By")]
        public string RejectedBy { get; set; }

        [DisplayName("Rejected On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RejectedOn { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("Is Deleted")]
        public bool? IsDeleted { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName("Is Active")]
        public bool? IsActive { get; set; }

        public string CreatedOnString { get; set; }

        [DisplayName("Status")]
        public string StatusApprovedDescription { get; set; }
        public virtual List<TrUserApproval> ListUserApprovals { get; set; }
    }

    public enum StatusApproved
    {
        CurrentApproval = 1,
        WaitingApproval,
        Approved,
        Rejected
    }
}