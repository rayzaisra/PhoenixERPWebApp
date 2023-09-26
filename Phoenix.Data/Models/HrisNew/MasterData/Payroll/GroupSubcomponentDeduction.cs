using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("group_subcomponent_deduction", Schema = "hr")]
    public class GroupSubcomponentDeduction : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_payroll_group_id"), StringLength(50)]
        public string EmployeePayrollGroupId { get; set; }

        [Column("salary_component")]
        public int? SalaryComponent { get; set; }

        [MaxLength(50)]
        [Column("component_id"), StringLength(50)]
        public string ComponentId { get; set; }

        [Column("deduction_percent")]
        public decimal? Percent { get; set; }

        [Column("deduction_amount")]
        public decimal? Amount { get; set; }

        [NotMapped]
        public string ComponentTypeName { get; set; }
    }
}