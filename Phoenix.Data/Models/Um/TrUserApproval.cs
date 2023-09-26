using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TrUserApproval", Schema = "Um")]
    public class TrUserApproval
    {

        public TrUserApproval() { }

        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsCondition { get; set; }
        public string TrTempApprovalId { get; set; }
        public int? IndexUser { get; set; }
        public StatusApproved StatusApproved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public bool? IsSpecialCase { get; set; }
        public string RemarkSpecialCase { get; set; }
        [Column("notification_type_id")]
        public string NotificationTypeId { get; set; }
        [Column("module")]
        public string Module { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }
        
        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public string Jobtitle { get; set; }
        [NotMapped]
        public string JobTitleName { get; set; }

        [NotMapped]
        public string StatusApprovedDescription { get { return StatusTransaction.StatusName(StatusApproved.GetHashCode()); } }

        //public string StatusApprovedDescription
        //{
        //    get
        //    {
        //        var stat = "";
        //        if (StatusApproved == StatusApproved.WaitingApproval) stat = "Waiting Approval";
        //        else if (StatusApproved == StatusApproved.Approved) stat = "Approved";
        //        else if (StatusApproved == StatusApproved.Rejected) stat = "Rejected";
        //        else stat = "Current Approval";
        //        return stat;
        //    }
        //}

        [NotMapped]
        public virtual List<TrConditionUserApproval> ListConditionalApprovals { get; set; }
    }

}
