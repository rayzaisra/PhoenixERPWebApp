using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("allowance_component", Schema = "hr")]
    public class AllowanceComponent : HrisEntityBase
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

        [Column(nameof(Value))]
        public decimal Value { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [NotMapped]
        public string JobGradeName { get; set; }
        [Column("is_salary_report")]
        public bool? Is_Salary_Report { get; set; }
    }
}