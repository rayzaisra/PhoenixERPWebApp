using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("TmUserApp", Schema ="um")]
    public partial class TmUserApp : BaseEntity<string>
    {
        public string AliasName { get; set; }
        public string Email { get; set; }
        public string UserPwd { get; set; }
        public string GroupId { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsMsUser { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }

    }
}
