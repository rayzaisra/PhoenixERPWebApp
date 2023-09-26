using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_category", Schema = "hr")]
    public class PdrCategory : HrisEntityBase
    {
        public PdrCategory()
        {
            PdrParameters = new List<PdrParameter>();
        }

        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column(nameof(No))]
        public int? No { get; set; }

        [MaxLength(100)]
        [Column(nameof(Name)), StringLength(100)]
        public string Name { get; set; }

        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }

        [Column("percentage")]
        public decimal? Percentage { get; set; }

        public List<PdrParameter> PdrParameters { get; set; }
    }
}