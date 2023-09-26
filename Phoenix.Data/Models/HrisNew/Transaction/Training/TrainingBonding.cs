using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("training_requisition_bonding", Schema = "hr")]
    public class TrainingBonding : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("training_req_id"), StringLength(50)]
        public string TrainingRequisitionId { get; set; }

        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }      

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("start_date"), StringLength(50)]
        public DateTime? StartDate { get; set; }

        [Column("end_date"), StringLength(50)]
        public DateTime? EndDate { get; set; }

        [Column("status")]
        public string Status { get; set; }

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
        public string TrainingName { get; set; }
        [NotMapped]
        public string TrainingType { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
     
        [NotMapped]
        public string Venue { get; set; }

        [NotMapped]
        public string GradeName { get; set; }
        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public string TrainingRequestCode { get; set; }
    }
  
}