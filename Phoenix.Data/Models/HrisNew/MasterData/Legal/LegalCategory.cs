using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("legal_category", Schema = "hr")]
    public class LegalCategory : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(nameof(Category)), StringLength(50)]
        public string Category { get; set; }

        [Required]
        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }
    }
}