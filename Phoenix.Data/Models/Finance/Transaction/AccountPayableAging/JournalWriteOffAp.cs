using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("journal_writeoff_ap", Schema = "fn")]
    public class JournalWriteOffAp : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("jv_number"), StringLength(50)]
        public string Code { get; set; }

        [Column("jv_date")]
        public DateTime? JvDate { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(250)]
        [Column("jv_reference_no"), StringLength(250)]
        public string JvReferenceNo { get; set; }

        [MaxLength(50)]
        [Column("jv_description"), StringLength(50)]
        public string JvDescription { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("receivable_account"), StringLength(50)]
        public string ReceivableAccount { get; set; }

        [MaxLength(50)]
        [Column("writeoff_account"), StringLength(50)]
        public string WriteOffAccount { get; set; }

        [Column("is_draft")]
        public bool? IsDraft { get; set; } 
    }
}