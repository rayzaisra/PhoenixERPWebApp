using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("bank_received_detail", Schema = "fn")]
    public class BankReceivedDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("bank_received_id")]
        public string BankReceivedId { get; set; }

        [Column("account_receivable_number")]
        public string AccountReceivebleNumber { get; set; }

        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }

        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }

        [Column("affilition_id")]
        public string AffiliationId { get; set; }

        [Column("account_id")]
        public string AccountId { get; set; }

        [Column("brand_id")]
        public string BrandId { get; set; }

        [Column("job_id")]
        public string JobId { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("currency_id")]
        public string CurrencyId { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("bank_account_destination")]
        public string BankAccountDestination { get; set; }

        [Column("charges")]
        public decimal? Charges { get; set; }

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("tax_payable")]
        public string TaxPayable { get; set; }

        [Column("source_doc")]
        public string SourceDoc { get; set; }

        [Column("tax_payable_coa_id")]
        public string TaxPayableCoa { get; set; }

        [NotMapped]
        public string BankAccountDestinationName { get; set; }

        [NotMapped]
        public string LegalEntity { get; set; }

        [NotMapped]
        public string ClientBrand { get; set; }

        [NotMapped]
        public string ClientName { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }

        [NotMapped]
        public string JobType { get; set; }
        [NotMapped]
        public string JobName { get; set; }
        [NotMapped]
        public string CurrencyCode { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
