using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TrTemplateApproval", Schema = "Um")]
    public class TrTemplateApproval
    {
        public TrTemplateApproval() { }

        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        public string RefId { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int? DueDate { get; set; }
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public StatusApproved StatusApproved { get; set; }
        public string RemarkRejected { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string DetailLink { get; set; }
        public string FormReqName { get; set; }

        public string ForDivisionId { get; set; }

        [NotMapped]
        public string CreatedOnString { get { return Convert.ToDateTime(CreatedOn).ToString("dd-MM-yyyy hh:mm:ss"); } }

        [NotMapped]
        public string StatusApprovedDescription { get { return StatusTransaction.StatusName(StatusApproved.GetHashCode()); } }
        //public string StatusApprovedDescription {
        //    get {
        //        var stat = "";
        //        if (StatusApproved == StatusApproved.WaitingApproval) stat = "Waiting Approval";
        //        else if (StatusApproved == StatusApproved.Approved) stat = "Approved";
        //        else if (StatusApproved == StatusApproved.Rejected) stat = "Rejected";
        //        else stat = "Current Approval";
        //        return stat;
        //    }
        //}
        [NotMapped]
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
