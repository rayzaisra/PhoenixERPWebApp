using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Holiday), Schema = "hr")]
    public class Holiday : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(nameof(Name)), StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(nameof(Date))]
        public DateTime Date { get; set; }
    }
}