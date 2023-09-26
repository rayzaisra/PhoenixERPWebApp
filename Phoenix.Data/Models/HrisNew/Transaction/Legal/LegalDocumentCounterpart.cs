using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("legal_document_counterpart", Schema = "hr")]
    public class LegalDocumentCounterpart : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("counterpart_id"), StringLength(50)]
        public string CounterpartId { get; set; }

        [MaxLength(50)]
        [Column("legal_document_id"), StringLength(50)]
        public string LegalDocumentId { get; set; }

        [NotMapped]
        public string CounterpartName { get; set; }
    }
}