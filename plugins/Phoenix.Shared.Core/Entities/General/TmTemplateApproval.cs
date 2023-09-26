using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("TmTemplateApproval", Schema ="Um")]
    public partial class TmTemplateApproval : BaseEntity<string>
    {
        public string TemplateName { get;set;}
		public string SubGroupId { get;set;}
		public string DivisionId { get;set;}
		public bool? IsNotifyByEmail { get;set;}
		public bool? IsNotifyByWeb { get;set;}
		public int? DueDate { get;set;}
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string JobTitleId { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string MenuId { get; set; }
		public string ForDivisionId { get; set; }
        public bool? IsAllDivision { get; set; }
        public bool? IsSpecialCase { get; set; }
    }
}