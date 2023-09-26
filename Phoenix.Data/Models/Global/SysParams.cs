using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("sys_params", Schema = "dbo")]
    public class SysParams
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("basic_salary_id"), StringLength(50)]
        public string BasicSalaryId { get; set; }

        [MaxLength(10)]
        [Column("crontab_payroll"), StringLength(10)]
        public string CrontabPayroll { get; set; }
    }
}