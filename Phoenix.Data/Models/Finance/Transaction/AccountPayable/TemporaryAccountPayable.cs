using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("account_payable_temporary", Schema = "fn")]
    public class TemporaryAccountPayable : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("tap_number"), StringLength(50)]
        public string TapNumber { get; set; }

        [MaxLength(50)]
        [Column("invoice_receivable_id"), StringLength(50)]
        public string InvoiceReceivableId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affilition_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column("bank_account_destination"), StringLength(50)]
        public string BankAccountDestination { get; set; }

        [Column(nameof(Charges))]
        public decimal? Charges { get; set; }

        [MaxLength(450)]
        [Column(nameof(Remarks)), StringLength(450)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        [Column("is_journal"), StringLength(50)]
        public string IsJournal { get; set; }

        [MaxLength(50)]
        [Column("d_k"), StringLength(50)]
        public string DK { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainServiceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("company_brand_id"), StringLength(50)]
        public string CompanyBrandId { get; set; }

        [MaxLength(50)]
        [Column("tax_number"), StringLength(50)]
        public string TaxNumber { get; set; }

        [Column(nameof(Periode))]
        public DateTime? Periode { get; set; }

        [Column("tax_date")]
        public DateTime? TaxDate { get; set; }

        [MaxLength(50)]
        [Column("invoice_vendor_number"), StringLength(50)]
        public string InvoiceVendorNumber { get; set; }

        [MaxLength(50)]
        [Column(nameof(APType)), StringLength(50)]
        public string APType { get; set; }

        [MaxLength(50)]
        [Column("allocation_ratio_id"), StringLength(50)]
        public string AllocationRatioId { get; set; }

        [Column("invoice_received_date")]
        public DateTime? InvoiceReceivedDate { get; set; }

        [MaxLength(50)]
        [Column(nameof(JobName)), StringLength(50)]
        public string JobName { get; set; }

        [MaxLength(50)]
        [Column(nameof(OurRef)), StringLength(50)]
        public string OurRef { get; set; }

        [MaxLength(50)]
        [Column(nameof(OurRefId)), StringLength(50)]
        public string OurRefId { get; set; }

        [Column("due_date")]
        public decimal? DueDays { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_id"), StringLength(50)]
        public string PurchaseOrderId { get; set; }

        [MaxLength(50)]
        [Column("line_code"), StringLength(50)]
        public string LineCode { get; set; }
    }
}
