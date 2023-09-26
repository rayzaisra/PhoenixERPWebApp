using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("account_payable", Schema = "fn")]
    public class AccountPayable : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("account_payable_number"), StringLength(50)]
        public string AccountPayableNumber { get; set; }

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
        [Column("invoice_receivable_id"), StringLength(50)]
        public string InvoiceReceivableId { get; set; }

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
        public string MainserviceCategoryId { get; set; }

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
        [Column("your_ref"), StringLength(50)]
        public string YourRef { get; set; }

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

        [MaxLength(50)]
        [Column("temporary_account_payable_id"), StringLength(50)]
        public string TemporaryAccountPayableId { get; set; }

        [Column("is_payment")]
        public bool? IsPayment { get; set; }


        //[NotMapped]
        //public List<AccountPayableDetail> Details { get; set; }

        ////For Payment Request
        //[NotMapped]
        //public string ClientName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string VendorName { get; set; }

        [NotMapped]
        public decimal? Vat { get; set; }
        [NotMapped]
        public decimal? TaxPayable { get; set; }

        [NotMapped]
        public string VendorAccountNumber { get; set; }

        [NotMapped]
        public string TaxPayableCoaId { get; set; }
        [NotMapped]
        public string TaxPayableCoaName { get; set; }

        [NotMapped]
        public string SwiftCode { get; set; }


        //[NotMapped]
        //public string PaymentRequestNumber { get; set; }
        //[NotMapped]
        //public decimal? PaymentRequestAmount { get; set; }
        //[NotMapped]
        //public string PaymentRequestStatus { get; set; }

        //[NotMapped]
        //public string PaymentRequestStatusDesc { get { return Phoenix.Data.Models.PaymentRequestStatus.GetStatus(this.PaymentRequestStatus); } }

        [NotMapped]
        public string MainServiceCategoryName { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencyIdDefault { get; set; }

        [NotMapped]
        public string CurrencySymbolDefault { get; set; }
        
        [NotMapped]
        public decimal? AmountOriginal { get; set; }

        [NotMapped]
        public decimal? TaxPayableOriginal { get; set; }
        [NotMapped]
        public decimal? VatOriginal { get; set; }
    }

    public class AccountPayableList
    {
        public string Id { get; set; }
        public string InvoiceReceivableId { get; set; }
        public string AccountPayableNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string InvoiceVendorNumber { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public string BusinessUnitName { get; set; }
        public string LegalEntityName { get; set; }
        public string PurchaseOrderId { get; set; }
        public string PaymentRequestNumber { get; set; }
        public string PaymentRequestStatus { get; set; }
        public decimal? PaymentRequestAmount { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string MainServiceCategoryName { get; set; }
        public decimal? AmountOriginal { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencySymbolDefault { get; set; }
    }
}