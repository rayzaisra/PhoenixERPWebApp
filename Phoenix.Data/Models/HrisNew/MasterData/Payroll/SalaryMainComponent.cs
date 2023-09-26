using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_main_component", Schema = "hr")]
    public class SalaryMainComponent : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("sub_component_name"), StringLength(50)]
        public string SubComponentName { get; set; }

        [MaxLength(100)]
        [Column(nameof(Description)), StringLength(100)]
        public string Description { get; set; }

        [Column("is_tax_calculate")]
        public bool? IsTaxCalculate { get; set; }

        [Column("is_salary")]
        public bool? IsSalary { get; set; }

        [Column("is_prorate_calculate")]
        public bool? IsProrateCalculate { get; set; }
        [Column("is_salary_report")]
        public bool? Is_Salary_Report { get; set; }
    }
}