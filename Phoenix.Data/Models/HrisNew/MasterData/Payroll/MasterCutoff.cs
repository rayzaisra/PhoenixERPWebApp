using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_cutoff", Schema = "hr")]
    public class MasterCutoff : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column(nameof(Name)), StringLength(100)]
        public string Name { get; set; }

        [Column("start_range")]
        public int? Start { get; set; }

        [Column("end_range")]
        public int? End { get; set; }
    }
}