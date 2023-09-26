using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmTemplateUserApproval", Schema = "Um")]
    public class TmTemplateUserApproval
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

        public string EmployeeBasicInfoId { get; set; }
        public int IndexUser { get; set; }
        public bool? IsCondition { get; set; }
        public string TmTempApprovalId { get; set; }

        public bool? IsSpecialCase { get; set; }
        public string RemarkSpecialCase { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public List<TmConditionUserApproval> ListConditionalApprovals { get; set; }
        [NotMapped]
        public string Email { get; set; }

    }
}
