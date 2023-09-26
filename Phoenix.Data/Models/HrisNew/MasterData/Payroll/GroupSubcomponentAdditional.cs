using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("group_subcomponent_additional", Schema = "hr")]
    public class GroupSubcomponentAdditional : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_payroll_group_id"), StringLength(50)]
        public string AdditionalEmployeePayrollGroupId { get; set; }

        [Column("salary_component")]
        public int? AdditionalSalaryComponent { get; set; }

        [MaxLength(50)]
        [Column("component_id"), StringLength(50)]
        public string AdditionalComponentId { get; set; }

        [Column("additional_percent")]
        public decimal? AdditionalPercent { get; set; }

        [Column("additional_amount")]
        public decimal? AdditionalAmount { get; set; }

        [NotMapped]
        public string AdditionalComponentTypeName { get; set; }
    }
}