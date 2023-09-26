using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("rotation_subcomponent_deduction", Schema = "hr")]
    public class RotationSubcomponentDeduction : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("salary_component")]
        public int? SalaryComponent { get; set; }

        [MaxLength(50)]
        [Column("component_id"), StringLength(50)]
        public string ComponentId { get; set; }

        [Column("deduction_percent")]
        public decimal? DeductionPercent { get; set; }

        [Column("deduction_amount")]
        public decimal? DeductionAmount { get; set; }

        [MaxLength(50)]
        [Column("rotation_id"), StringLength(50)]
        public string RotationId { get; set; }

        [Column("type")] // 1. Current Salary, 2. New Salary
        public int? Type { get; set; }

        [NotMapped]
        public string ComponentTypeName { get; set; }

        [NotMapped]
        public bool IsBasicSalary { get; set; }
    }
}