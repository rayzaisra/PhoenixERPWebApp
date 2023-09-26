using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_payroll_group", Schema = "hr")]
    public class EmployeePayrollGroup : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("group_name"), StringLength(50)]
        public string GroupName { get; set; }

        [NotMapped]
        public List<GroupSubcomponentAdditional> GroupSubcomponentAdditionals { get; set; }
        [NotMapped]
        public List<GroupSubcomponentDeduction> GroupSubcomponentDeductions { get; set; }
    }
}