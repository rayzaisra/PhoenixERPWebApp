using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("termination_type", Schema = "hr")]
    public class TmTerminationType : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("type_name"), StringLength(100)]
        public string TypeName { get; set; }
    }
}