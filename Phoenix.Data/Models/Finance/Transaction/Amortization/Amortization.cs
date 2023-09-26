using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("amortization", Schema = "fn")]
    public class Amortization : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("amotization_number"), StringLength(50)]
        public string Code { get; set; }

        [Column("amotization_date")]
        public DateTime? AmotizationDate { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        //[MaxLength(250)]
        //[Column("amotization_reference_no"), StringLength(250)]
        //public string AmotizationReferenceNo { get; set; }

        [MaxLength(50)]
        [Column("amotization_description"), StringLength(50)]
        public string AmotizationDescription { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [Column("is_draft")]
        public bool? IsDraft { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Condition)), StringLength(50)]
        public string Condition { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        [Column("cost_sharing_allocation_id"), StringLength(50)]
        public string CostSharingAllocationId { get; set; }

        [NotMapped]
        public List<AmortizationDetail> AmortizationDetails { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public DateTime? AmotizationDateEnd { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string AfiliationName { get; set; }
    }
}