using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("training_requisition_detail", Schema = "hr")]
    public class TrainingRequisitionDetail : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("training_requition_id"), StringLength(50)]
        public string TrainingRequisitionId { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }
        
        [NotMapped]
        public string JobTitleId { get; set; }
        [NotMapped]
        public string JobTitleName { get; set; }
        [NotMapped]
        public string JobGradeId { get; set; }
        [NotMapped]
        public string JobGradeName { get; set; }
        [NotMapped]
        public string DivisionId { get; set; }
        [NotMapped]
        public string DivisionName { get; set; }
        [NotMapped]
        public string EmployeeBasicInfoName { get; set; }
        [NotMapped]
        public string GroupId { get; set; }
        [NotMapped]
        public string SubgroupId { get; set; }
        [NotMapped]
        public string DepartementId { get; set; }
        [NotMapped]
        public string ApprovalId { get; set; }
    }

    public class TrainingRequisitionDetailFilter {

        public string FilterGroupId { get; set; }
        public string FilterSubgroupId { get; set; }
        public string FilterDivisionId { get; set; }
        public string FilterDepartementId { get; set; }
    }

  
}