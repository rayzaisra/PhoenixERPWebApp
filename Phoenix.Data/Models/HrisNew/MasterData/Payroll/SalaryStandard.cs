using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_standard", Schema = "hr")]
    public class SalaryStandard : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(150)]
        [Column(nameof(Name)), StringLength(150)]
        public string Name { get; set; }

        [Column(nameof(Value))]
        public decimal Value { get; set; }

        [MaxLength(255)]
        [Column(nameof(Description)), StringLength(255)]
        public string Description { get; set; }
    }
}