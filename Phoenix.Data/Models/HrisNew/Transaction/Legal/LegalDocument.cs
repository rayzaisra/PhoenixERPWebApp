using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("legal_document", Schema = "hr")]
    public class LegalDocument : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column(nameof(Category)), StringLength(50)]
        public string Category { get; set; }

        [MaxLength(50)]
        [Column("legal_category_id"), StringLength(50)]
        public string LegalCategoryId { get; set; }

        [Column("document_date")]
        public DateTime? DocumentDate { get; set; }

        [Column("expired_date")]
        public DateTime? ExpiredDate { get; set; }

        [MaxLength(100)]
        [Column("file_upload"), StringLength(100)]
        public string FileUpload { get; set; }

        [MaxLength(100)]
        [Column(nameof(Label)), StringLength(100)]
        public string Label { get; set; }

        [Column(nameof(Remarks)), StringLength(2147483647)]
        public string Remarks { get; set; }

        [NotMapped]
        public LegalDocumentCounterpart LegalDocumentCounterpart { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
        [NotMapped]
        public string FileUploadName { get; set; }
        [NotMapped]
        public string LegalCategoryName { get; set; }
        [NotMapped]
        public string LocationName { get; set; }
        [NotMapped]
        public string DivisionId { get; set; }
        [NotMapped]
        public string DivisionName { get; set; }
    }

    public class LegalDocumentSearch
    {
        public string Category { get; set; }
        public string LegalCategoryId { get; set; }
        public string CounterpartId { get; set; }
    }

    public class DashboardSearch : LegalDocumentSearch
    {
        public string DivisionId { get; set; }
        public string LocationId { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}