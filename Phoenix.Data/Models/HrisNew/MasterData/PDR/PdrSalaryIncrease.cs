using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_salary_increase", Schema = "hr")]
    public class PdrSalaryIncrease : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_grade_id"), StringLength(50)]
        public string PdrGradeId { get; set; }

        //[Column("salary_increase")]
        //public decimal? SalaryIncrease { get; set; }
        [Column("salary_increase")]
        public double? SalaryIncrease { get; set; }

        [NotMapped]
        public string PdrGrade { get; set; }

        [NotMapped]
        public string PdrSalaryGradeName { get; set; }

        [NotMapped]
        public List<string> PdrSalaryGrade { get; set; }
    }
}