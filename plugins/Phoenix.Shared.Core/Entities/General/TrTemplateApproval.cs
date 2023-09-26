using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("TrTemplateApproval", Schema ="Um")]
    public partial class TrTemplateApproval :BaseEntity<string>
    {
        public string RefId { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get;set;}
		public int? DueDate { get;set;}
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public int? StatusApproved { get;set;}
		public string RemarkRejected { get;set;}
		public string DetailLink { get;set;}
		public string FormReqName { get;set;}
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}