using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("bsc_corporate_objective", Schema = "hr")]
    public class BscCorporateObjective : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("bsc_category"), StringLength(50)]
        public string BscCategory { get; set; }

        [MaxLength(100)]
        [Column("bsc_name"), StringLength(100)]
        public string BscName { get; set; }

        [MaxLength(50)]
        [Column("parent_id"), StringLength(50)]
        public string ParentId { get; set; }

        [MaxLength(4)]
        [Column(nameof(Year)), StringLength(4)]
        public string Year { get; set; }

        [NotMapped]
        public string ParentName { get; set; }
    }
}