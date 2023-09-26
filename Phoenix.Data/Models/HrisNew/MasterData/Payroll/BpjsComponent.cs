using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("bpjs_component", Schema = "hr")]
    public class BpjsComponent : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("sub_component_name"), StringLength(50)]
        public string SubComponentName { get; set; }

        [Column(nameof(Responsibility))]
        public int Responsibility { get; set; }

        [Column(nameof(Value))]
        public decimal Value { get; set; }

        [Column("value_corporate")]
        public decimal ValueCorporate { get; set; }

        [Column("max_limit")]
        public decimal MaxLimit { get; set; }

        [Column("is_tax")]
        public bool? IsTax { get; set; }

        [MaxLength(100)]
        [Column(nameof(Description)), StringLength(100)]
        public string Description { get; set; }

        [Column(nameof(Type))]
        public int? Type { get; set; }
        [Column("is_salary_report")]
        public bool? Is_Salary_Report { get; set; }
    }
}