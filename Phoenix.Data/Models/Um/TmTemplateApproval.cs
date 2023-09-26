using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmTemplateApproval", Schema = "Um")]
    public class TmTemplateApproval
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

        public string TemplateName { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public bool? IsAllDivision { get; set; }
        public bool? IsSpecialCase { get; set; }
        public int DueDate { get; set; }
        public int Reminder { get; set; }
        public int ApprovalCategory { get; set; }
        public int ApprovalType { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public string JobTitleId { get; set; }
        public string MenuId { get; set; }

        //public string BussinessUnitDivisionId { get; set; }
        //public string BussinessUnitJobLevelId { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }

        [NotMapped]
        public List<TmTemplateUserApproval> ListUserApprovals { get; set; }

        [NotMapped]
        public string MenuName { get; set; }
        [NotMapped]
        public string JobTitleName { get; set; }
        public string ForDivisionId { get; set; }

        [NotMapped]
        public string ForDivisionName { get; set; }


    }
}
