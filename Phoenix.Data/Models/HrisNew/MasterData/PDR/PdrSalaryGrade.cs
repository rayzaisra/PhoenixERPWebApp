using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_salary_grade", Schema = "hr")]
    public class PdrSalaryGrade : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [MaxLength(50)]
        [Column("pdr_salary_increase_id"), StringLength(50)]
        public string PdrSalaryIncreaseId { get; set; }

        [NotMapped]
        public string GradeName { get; set; }
    }
}