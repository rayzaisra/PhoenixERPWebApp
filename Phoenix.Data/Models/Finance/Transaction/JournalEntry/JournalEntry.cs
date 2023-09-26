using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("journal_entry", Schema = "fn")]
    public class JournalEntry : FinanceEntityBase
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

        //[MaxLength(250)]
        //[Column("jv_reference_no"), StringLength(250)]
        //public string JvReferenceNo { get; set; }

        [MaxLength(50)]
        [Column("jv_description"), StringLength(50)]
        public string JvDescription { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Column("is_draft")]
        public bool? IsDraft { get; set; }

        [NotMapped]
        public List<JournalEntryDetail> JournalEntryDetails { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public DateTime? JvDateEnd { get; set; }
    }
}