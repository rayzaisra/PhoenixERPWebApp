using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("legal_document_request", Schema = "hr")]
    public class LegalDocumentRequest : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column("legal_category_id"), StringLength(50)]
        public string LegalCategoryId { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [MaxLength(50)]
        [Column("legal_doc_file"), StringLength(50)]
        public string LegalDocFile { get; set; }

        [MaxLength(255)]
        [Column(nameof(Remarks)), StringLength(255)]
        public string Remarks { get; set; }

        [Column("submit_user")]
        public bool? SubmitUser { get; set; }

        public LegalCategory LegalCategory { get; set; }

        [NotMapped]
        public string RequestBy { get; set; }

        [NotMapped]
        public string Counterpart { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }

        [NotMapped]
        public string LegalDocFileName { get; set; }
    }
}