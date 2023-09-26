using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("ptkp_setting", Schema = "hr")]
    public class PtkpSetting : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Name)), StringLength(50)]
        public string Name { get; set; }

        [Column(nameof(Value))]
        public decimal Value { get; set; }

        [MaxLength(255)]
        [Column(nameof(Description)), StringLength(255)]
        public string Description { get; set; }
    }
}