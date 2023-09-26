using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("promotion_subcomponent_additional", Schema = "hr")]
    public class PromotionSubcomponentAdditional : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string AdditionalEmployeeBasicInfoId { get; set; }

        [Column("salary_component")]
        public int? AdditionalSalaryComponent { get; set; }

        [MaxLength(50)]
        [Column("component_id"), StringLength(50)]
        public string AdditionalComponentId { get; set; }

        [Column("additional_percent")]
        public decimal? AdditionalPercent { get; set; }

        [Column("additional_amount")]
        public decimal? AdditionalAmount { get; set; }

        [MaxLength(50)]
        [Column("promotion_id"), StringLength(50)]
        public string AdditionalPromotionId { get; set; }

        [Column("type")] // 1. Current Salary, 2. New Salary
        public int? Type { get; set; }

        [NotMapped]
        public string AdditionalComponentTypeName { get; set; }

        [NotMapped]
        public bool IsBasicSalary { get; set; }
    }
}