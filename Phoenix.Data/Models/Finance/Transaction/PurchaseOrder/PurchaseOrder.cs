using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("purchase_order", Schema = "fn")]
    public class PurchaseOrder : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_number"), StringLength(50)]
        public string PurchaseOrderNumber { get; set; }

        [Column("purchase_order_date")]
        public DateTime? PurchaseOrderDate { get; set; }

        [Column("delivery_date")]
        public DateTime? DeliveryDate { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("purchase_request_id"), StringLength(50)]
        public string PurchaseRequestId { get; set; }

        [MaxLength(50)]
        [Column("purchase_request_number"), StringLength(50)]
        public string PurchaseRequestNumber { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("job_number"), StringLength(50)]
        public string JobNumber { get; set; }

        [MaxLength(50)]
        [Column("pic_id"), StringLength(50)]
        public string PicId { get; set; }

        [MaxLength(50)]
        [Column("pic_email"), StringLength(50)]
        public string PicEmail { get; set; }

        [MaxLength(50)]
        [Column("company_id"), StringLength(50)]
        public string CompanyId { get; set; }

        [MaxLength(400)]
        [Column("company_name"), StringLength(400)]
        public string CompanyName { get; set; }

        [MaxLength(350)]
        [Column("company_reference"), StringLength(350)]
        public string CompanyReference { get; set; }

        [MaxLength(50)]
        [Column("pic_name"), StringLength(50)]
        public string VendorPicName { get; set; }

        [MaxLength(150)]
        [Column("number_npwp"), StringLength(150)]
        public string NumberNpwp { get; set; }

        [MaxLength(50)]
        [Column("company_brand_id"), StringLength(50)]
        public string CompanyBrandId { get; set; }

        [MaxLength(450)]
        [Column(nameof(Description)), StringLength(450)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("discount_percent")]
        public decimal? DiscountPercent { get; set; }

        [Column("discount_amount")]
        public decimal? DiscountAmount { get; set; }

        //[Column("tax_amount")]
        //public decimal? TaxAmount { get; set; }

        //[Column("tax_percent")]
        //public decimal? TaxPercent { get; set; }

        //[Column(nameof(Amount))]
        //public decimal? Amount { get; set; }

        [Column("sub_total")]
        public decimal? SubTotal { get; set; }

        [Column("vat_percent")]
        public decimal? VatPercent { get; set; }

        [Column(nameof(Vat))]
        public decimal? Vat { get; set; }

        [Column("grand_total")]
        public decimal? GrandTotal { get; set; }

        [Column("percent_payment_term1")]
        public decimal? PercentPaymentTerm1 { get; set; }

        [Column("percent_payment_term2")]
        public decimal? PercentPaymentTerm2 { get; set; }

        [Column("percent_payment_term3")]
        public decimal? PercentPaymentTerm3 { get; set; }

        [Column("payment_term1")]
        public decimal? PaymentTerm1 { get; set; }

        [Column("payment_term2")]
        public decimal? PaymentTerm2 { get; set; }

        [Column("payment_term3")]
        public decimal? PaymentTerm3 { get; set; }

        [MaxLength(50)]
        [Column("invoicing_type"), StringLength(50)]
        public string InvoicingType { get; set; }

        [MaxLength(50)]
        [Column("payment_method_id"), StringLength(50)]
        public string PaymentMethodId { get; set; }

        [MaxLength(50)]
        [Column("special_request_id"), StringLength(50)]
        public string SpecialRequestId { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("request_for_quotation_id"), StringLength(50)]
        public string RequestForQuotationId { get; set; }

        [MaxLength(150)]
        [Column("request_for_quotation_number"), StringLength(150)]
        public string RequestForQuotationNumber { get; set; }

        [MaxLength(50)]
        [Column("cash_advance_id"), StringLength(50)]
        public string CashAdvanceId { get; set; }

        [MaxLength(50)]
        [Column("payment_request_id"), StringLength(50)]
        public string PaymentRequestId { get; set; }

        [MaxLength(50)]
        [Column("cash_advance_settlement_id"), StringLength(50)]
        public string CashAdvanceSettlementId { get; set; }

        [MaxLength(50)]
        [Column("type_of_expense_id"), StringLength(50)]
        public string TypeOfExpenseId { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [NotMapped]
        public string RequestorName { get; set; }

        [NotMapped]
        public string JobPaId { get; set; }

        [NotMapped]
        public string JobPeId { get; set; }

        [NotMapped]
        public string JobName { get; set; }

        [NotMapped]
        public List<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }

        //Info PCA
        [NotMapped]
        public string ShareServicesId { get; set; }

        //info rfq
        [NotMapped]
        public decimal? SubTotalRFQ { get; set; }

        [NotMapped]
        public decimal? VatPercentRFQ { get; set; }

        [NotMapped]
        public decimal? VatRFQ { get; set; }

        [NotMapped]
        public decimal? GrandTotalRFQ { get; set; }

        [NotMapped]
        public int? StatusApproval { get; set; }

        [NotMapped]
        public string RemarksProcess { get; set; }

        [NotMapped]
        public bool IsCancel { get; set; }

        [NotMapped]
        public bool IsClose { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolExchange { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }
    }
}