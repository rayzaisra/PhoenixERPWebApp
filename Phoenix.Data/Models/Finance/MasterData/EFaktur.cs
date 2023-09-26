using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("e_faktur", Schema = "fn")]
    public class EFaktur : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(nameof(Name)), StringLength(150)]
        public string Name { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("start_number"), StringLength(50)]
        public string StartNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("end_number"), StringLength(50)]
        public string EndNumber { get; set; }

        [NotMapped]
        public int Used { get; set; }

        [NotMapped]
        public int Remaining { get; set; }
    }
}