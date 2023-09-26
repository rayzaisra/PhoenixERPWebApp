using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("TmMenu",Schema ="Um")]
    public partial class TmMenu : BaseEntity<string>
    {
        public string MenuName { get;set;}
		public string family_status_id {get;set;}
		public string MenuUnique { get;set;}
		public string ParentId { get;set;}
		public bool? IsDeleted { get;set;}
		public string CreatedBy { get;set;}
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}