using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmConditionUserApproval", Schema = "Um")]
    public class TmConditionUserApproval
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
        public int GroupConditionIndex { get; set; }
        public string GroupConditionApproval { get; set; }
        public string ConditionalApproval { get; set; }
        public string TemplateUserApprovalId { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }

    }
}
