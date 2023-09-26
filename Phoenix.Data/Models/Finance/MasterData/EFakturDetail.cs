using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("e_faktur_detail", Schema = "fn")]
    public class EFakturDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("e_faktur_id"), StringLength(50)]
        public string EFakturId { get; set; }

        [MaxLength(150)]
        [Column("vat_no"), StringLength(150)]
        public string vat_no { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; }

        [MaxLength(50)]
        [Column("reff_number"), StringLength(50)]
        public string ReffNumber { get; set; }
    }
}