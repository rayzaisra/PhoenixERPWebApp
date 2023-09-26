using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("legal_sharing_request", Schema = "hr")]
    public class LegalSharingRequest : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("legal_document_id"), StringLength(50)]
        public string LegalDocumentId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Category)), StringLength(50)]
        public string Category { get; set; }

        [MaxLength(255)]
        [Column("counterpart_id"), StringLength(255)]
        public string CounterpartId { get; set; }

        [Column("start_sharing")]
        public DateTime? StartSharing { get; set; }

        [Column("end_sharing")]
        public DateTime? EndSharing { get; set; }

        [MaxLength(255)]
        [Column(nameof(Purpose)), StringLength(255)]
        public string Purpose { get; set; }
        
        [NotMapped]
        public LegalDocument LegalDocument { get; set; }

        [NotMapped]
        public string RequestBy { get; set; }

        [NotMapped]
        public string CounterpartName { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }
    }
}