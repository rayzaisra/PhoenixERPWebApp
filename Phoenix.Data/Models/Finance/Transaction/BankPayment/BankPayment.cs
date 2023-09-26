using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("bank_payment", Schema = "fn")]
    public class BankPayment : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("payment_number"), StringLength(50)]
        public string PaymentNumber { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("bank_payment_method_id"), StringLength(50)]
        public string BankPaymentMethodId { get; set; }

        [Column("reference_number")]
        public string ReferenceNumber { get; set; }

        [Column("bank_account_destination")]
        public string BankAccountDestination { get; set; }

        [Column("payment_request_id")]
        public string PaymentRequestId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("bank_payment_type_id")]
        public string BankPaymentTypeId { get; set; }

        [Column("payment_amount")]
        public decimal? PaymentAmount { get; set; }

        [Column("payment_charges")]
        public decimal? PaymentCharges { get; set; }

        [Column("vendor_id")]
        public string VendorId { get; set; }

        [Column("account_id")]
        public string AccountId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(450)]
        [Column("remarks"), StringLength(450)]
        public string Remarks { get; set; }

        [MaxLength(450)]
        [Column("notes"), StringLength(450)]
        public string Notes { get; set; }

        [Column("approval_id")]
        public string ApprovalId { get; set; }


        [Column("bank_payment_status_id")]
        public string BankPaymentStatusId { get; set; }

        [MaxLength(50)]
        [Column("source_doc"), StringLength(50)]
        public string SourceDoc { get; set; }

        [MaxLength(50)]
        [Column("bank_id"), StringLength(50)]
        public string BankId { get; set; }

        [Column("vat_percent")]
        public decimal? VatPercent { get; set; }

        [Column("vat")]
        public decimal? Vat { get; set; }

        [Column("tax_payable")]
        public decimal? TaxPayable { get; set; }

        [Column("tax_payable_percent")]
        public decimal? TaxPayablePercent { get; set; }

        [Column("advertising_tax")]
        public decimal? AdvertisingTax { get; set; }

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }

        [Column("invoice_receivable_id")]
        public string InvoiceReceivableId { get; set; }

        [Column("reference_number_id")]
        public string ReferenceNumberId { get; set; }

        [Column("job_id")]
        public string JobId { get; set; }

        [Column("purchase_request_id")]
        public string PurchaseRequestId { get; set; }

        [Column("is_multiple")]
        public bool? IsMultiple { get; set; }

        [Column("requested_on")]
        public DateTime? RequestOn { get; set; }

        [Column("balance_amount")]
        public decimal? BalanceAmount { get; set; }

        [Column("account_id_tax_payable")]
        public string AccountIdTaxPayable { get; set; }

        [Column("account_id_bank_charges")]
        public string AccountIdBankCharges { get; set; }

        [Column("account_id_pay_to")]
        public string AccountIdPayTo { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string JobName { get; set; }
        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public string InvoiceReceivableNumber { get; set; }
    }
}
