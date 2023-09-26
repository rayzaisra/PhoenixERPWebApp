using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("vw_job_closing_ap", Schema = "fn")]
    public class VwJobClosingAp
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(100)]
        [Column("reference_id"), StringLength(100)]
        public string ReferenceId { get; set; }

        [MaxLength(50)]
        [Column("reff_number"), StringLength(50)]
        public string ReffNumber { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(20)]
        public string CoaId { get; set; }

        [MaxLength(20)]
        [Column("code_rec"), StringLength(20)]
        public string CodeRec { get; set; }

        [MaxLength(80)]
        [Column(nameof(Name5)), StringLength(80)]
        public string Name5 { get; set; }

        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }

        [MaxLength(50)]
        [Column("sharedservice_id"), StringLength(50)]
        public string SharedserviceId { get; set; }

        [MaxLength(150)]
        [Column("sharedservice_name"), StringLength(150)]
        public string SharedserviceName { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Unit)), StringLength(50)]
        public string Unit { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column("legal_id"), StringLength(50)]
        public string LegalId { get; set; }

        [MaxLength(50)]
        [Column("affiliate_id"), StringLength(50)]
        public string AffiliateId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }
    }
}
