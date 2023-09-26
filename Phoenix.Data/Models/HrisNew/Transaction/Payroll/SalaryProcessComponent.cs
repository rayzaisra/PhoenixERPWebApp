using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_process_component", Schema = "hr")]
    public class SalaryProcessComponent : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(100)]
        [Column("component_name"), StringLength(100)]
        public string ComponentName { get; set; }

        [Column(nameof(Value))]
        public decimal? Value { get; set; }

        [Column("value_year")]
        public decimal? ValueYear { get; set; }

        [MaxLength(100)]
        [Column(nameof(Type)), StringLength(100)]
        public string Type { get; set; }

        [Column("tax_id")]
        public long? TaxId { get; set; }

        [MaxLength(50)]
        [Column("salary_process_header_id"), StringLength(50)]
        public string SalaryProcessHeaderId { get; set; }
    }
}