using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("TrUserApproval", Schema ="Um")]
    public partial class TrUserApproval : BaseEntity<string>
    {
        public string EmployeeBasicInfoId { get;set;}
		public bool? IsCondition { get;set;}
		public string TrTempApprovalId { get;set;}
		public int? IndexUser { get;set;}
		public int? StatusApproved { get;set;}
		public string CreatedBy { get;set;}
        public DateTime? CreatedOn { get; set; }
        public bool? IsSpecialCase { get; set; }
        public string RemarkSpecialCase { get; set; }
    }
}