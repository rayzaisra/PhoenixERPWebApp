using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("job_closing_header", Schema = "fn")]
    public class JobClosingHeader : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("job_closing_number"), StringLength(50)]
        public string RequestCode { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("job_name"), StringLength(50)]
        public string JobName { get; set; }

        [MaxLength(50)]
        [Column("divisi_id"), StringLength(50)]
        public string DivisiId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(50)]
        [Column("brand_id"), StringLength(50)]
        public string BrandId { get; set; }

        [Column(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        [MaxLength(50)]
        [Column("job_status"), StringLength(50)]
        public string JobStatus { get; set; }

        [MaxLength(50)]
        [Column("finance_status"), StringLength(50)]
        public string FinanceStatus { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [NotMapped]
        public List<JobClosingDetail> JobClosingDetails { get; set; }

        [NotMapped]
        public List<JobClosingDetailJournal> JournalEntry { get; set; }

        [NotMapped]
        public int? StatusApproval { get; set; }

        [NotMapped]
        public string RemarksProcess { get; set; }

        [NotMapped]
        public string JobNumber { get; set; }

        [NotMapped]
        public string MainServiceCategoryId { get; set; }
    }
}