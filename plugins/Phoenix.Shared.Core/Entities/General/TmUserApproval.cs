using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("TmTemplateUserApproval", Schema ="Um")]
    public partial class TmUserApproval : BaseEntity<string>
    {
        public string EmployeeBasicInfoId { get;set;}
		public bool? IsCondition { get;set;}
		public string TmTempApprovalId { get;set;}
		public int? IndexUser { get;set;}
		public bool? IsDeleted { get;set;}
		public string CreatedBy { get;set;}
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }

    }
}