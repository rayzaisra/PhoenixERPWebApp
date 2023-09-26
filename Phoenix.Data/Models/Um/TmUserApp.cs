using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmUserApp", Schema = "Um")]
    public class TmUserApp 
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string AliasName { get; set; }
        public string GroupId { get; set; }
        public string Email { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsMsUser { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsUserNeedTimesheet { get; set; }
        public string UserPwd { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }

        public bool? IsFirstTimeLogin { get; set; }
        public bool? IsOnLine { get; set; }

        public string RawSession { get; set; }
        [NotMapped]
        public string GroupName { get; set; }
    }
}
