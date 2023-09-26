using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("category_exit_interview", Schema = "hr")]
    public class CategoryExitInterview : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column(nameof(No))]
        public int? No { get; set; }

        [MaxLength(100)]
        [Column(nameof(Name)), StringLength(100)]
        public string Name { get; set; }
    }
}