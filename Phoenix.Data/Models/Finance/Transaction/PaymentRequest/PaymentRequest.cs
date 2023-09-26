using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("payment_request", Schema = "fn")]
    public class PaymentRequest : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("payment_request_number"), StringLength(50)]
        public string PaymentRequestNumber { get; set; }

        [Column("payment_request_date")]
        public DateTime? PaymentRequestDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [MaxLength(50)]
        [Column("bank_payment_type_id"), StringLength(50)]
        public string PaymentTypeId { get; set; }

        [MaxLength(50)]
        [Column("BankName"), StringLength(50)]
        public string BankName { get; set; }

        [MaxLength(50)]
        [Column("bank_id"), StringLength(50)]
        public string BankId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffilicationId { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        //[Column("operator"), StringLength(50)]
        //public string Operator { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("charges")]
        public decimal? Charges { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Column("amount_pay")]
        public decimal? AmountPay { get; set; }

        [Column("total_pay_amount")]
        public decimal? TotalAmountPay { get; set; }

        [Column("vat")]
        public decimal? Vat { get; set; }

        [Column("vat_percent")]
        public decimal? VatPercent { get; set; }

        [Column("tax_payable")]
        public decimal? TaxPayable { get; set; }

        [Column("tax_payable_percent")]
        public decimal? TaxPayablePercent { get; set; }

        [Column("advertising_tax")]
        public decimal? AdvertisingTax { get; set; }

        [Column("invoice_receivable_id")]
        public string InvoiceReceivableId { get; set; }

        [Column("reference_number_id")]
        public string ReferenceNumberId { get; set; }

        [Column("paytoname")]
        public string PaytoName { get; set; }

        [Column("discount")]
        public decimal? Discount { get; set; }

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }
        [Column("job_id")]
        public string JobId { get; set; }

        [Column("purchase_request_id")]
        public string PurchaseRequestId { get; set; }
        [Column("mainservice_category_id")]
        public string MainServiceCategoryId { get; set; }
        [Column("source_doc")]
        public string SourceDoc { get; set; }

        [Column("recon")]
        public bool? Recon { get; set; }
        [Column("tax_pay_coa_id")]
        public string TaxPayCoaId { get; set; }
        [Column("paytobank")]
        public string PaytoBank { get; set; }

        [Column("payto_coa_id")]
        public string PaytoCoaId { get; set; }

        [Column("bank_charges_coa_id")]
        public string BankChargesCoaId { get; set; }

        [Column("balance")]
        public decimal? Balance { get; set; }

        [Column("is_multiple")]
        public bool? IsMultiple { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("pairing_group_id")]
        public string PairingGroupId { get; set; }

        [Column("is_balance")]
        public bool? IsBalance { get; set; }

        [Column("payto_pettycash")]
        public string PayToPettyCash { get; set; }
        [Column("payto_account_number")]
        public string PayToAccountNumber { get; set; }
        [Column("payto_type")]
        public string PayToType { get; set; }
        [Column("payto_vendor")]
        public string PayToVendor { get; set; }

        [Column("account_number")]
        public string AccountNumber { get; set; }

        [Column("tax_payable_coa")]
        public string TaxPayableCoa { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string JobName { get; set; }
        [NotMapped]
        public string LogDescription { get; set; }
        [NotMapped]
        public string InvoiceReceivableNumber { get; set; }

        [NotMapped]
        public string TempHeaderId { get; set; }

        [NotMapped]
        public string PaytoBankName { get; set; }

        [NotMapped]
        public List<PaymentRequestDetail> Details { get; set; }

        [NotMapped]
        public List<PaymentRequestDetailJournal> JournalEntry { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencyIdDefault { get; set; }

        [NotMapped]
        public string CurrencySymbolDefault { get; set; }
    }

    public class BankPaymentList
    {
        public string Id { get; set; }
        public DateTime? PaymentRequestDate { get; set; }
        public string PaymentRequestNumber { get; set; }
        public string ReferenceNumberId { get; set; }
        public string LegalEntityName { get; set; }
        public string VendorName { get; set; }
        public string JobName { get; set; }
        public string BusinessUnitName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrencyName { get; set; }
        public decimal? AmountPay { get; set; }
        public string Status { get; set; }
    }

    public class FilterPaymentRequest
    {
        public string Status { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string DateType { get; set; }
    }
}
