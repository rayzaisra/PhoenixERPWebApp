using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("job_closing_detail", Schema = "fn")]
    public class JobClosingDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("job_closing_header_id"), StringLength(50)]
        public string JobClosingHeaderId { get; set; }

        [MaxLength(50)]
        [Column("trx_code"), StringLength(50)]
        public string TrxCode { get; set; }

        [MaxLength(50)]
        [Column("journal_id"), StringLength(50)]
        public string JournalId { get; set; }

        [MaxLength(50)]
        [Column("reff_number"), StringLength(50)]
        public string ReffNumber { get; set; }

        [Column("reff_date")]
        public DateTime? ReffDate { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(500)]
        [Column(nameof(Description)), StringLength(500)]
        public string Description { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }
    }
}