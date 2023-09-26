using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("journal_entry_detail", Schema = "fn")]
    public class JournalEntryDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("journal_entry_id"), StringLength(50)]
        public string JournalEntryId { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Description)), StringLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column(nameof(Condition)), StringLength(50)]
        public string Condition { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }
        
        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }
               
        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }
        
        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("pce_id"), StringLength(50)]
        public string PceId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(50)]
        [Column("pca_task_id"), StringLength(50)]
        public string PcaTaskId { get; set; }

        [NotMapped]
        public string JobName { get; set; }

        [NotMapped]
        public string PcaTask { get; set; }

        [NotMapped]
        public string AccountName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }

        //[NotMapped]
        //public string CodeRec { get; set; }
        [NotMapped]
        public string AfiliationName { get; set; }
    }
}