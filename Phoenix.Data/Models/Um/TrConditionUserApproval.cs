using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TrConditionUserApproval", Schema = "Um")]
    public class TrConditionUserApproval
    {

        public TrConditionUserApproval() { }

        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public int? GroupConditionIndex { get; set; }
        public string GroupConditionApproval { get; set; }
        public string ConditionalApproval { get; set; }
        public string TrUserApprovalId { get; set; }
        public StatusApproved StatusApproved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }
    }


}
