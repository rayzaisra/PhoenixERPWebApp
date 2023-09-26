using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("vw_exec_journal_ap_header", Schema = "fn")]
    public class VwjournalAp
    {
        [Key]
        [Required]
        [MaxLength(50)]
        [Column("invoice_receivable_id"), StringLength(50)]
        public string InvoiceReceivableId { get; set; }

        [Column("invoice_date")]
        public DateTime? InvoiceDate { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("invoice_number"), StringLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("purchase_order_id"), StringLength(50)]
        public string PurchaseOrderId { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_number"), StringLength(50)]
        public string PurchaseOrderNumber { get; set; }

        [MaxLength(50)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [MaxLength(50)]
        [Column("invoice_vendor"), StringLength(50)]
        public string InvoiceVendor { get; set; }

        [Column("invoice_vendor_date")]
        public DateTime? InvoiceVendorDate { get; set; }

        [MaxLength(500)]
        [Column(nameof(Description)), StringLength(500)]
        public string Description { get; set; }

        [Column("service_name")]
        public int? ServiceName { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("shareservice_id"), StringLength(50)]
        public string ShareserviceId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("method_name"), StringLength(50)]
        public string MethodName { get; set; }

        [MaxLength(50)]
        [Column("tax_number"), StringLength(50)]
        public string TaxNumber { get; set; }

        [Column("tax_date")]
        public DateTime? TaxDate { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [MaxLength(50)]
        [Column("currency_code"), StringLength(50)]
        public string CurrencyCode { get; set; }

        [MaxLength(20)]
        [Column("code_rec"), StringLength(20)]
        public string CodeRec { get; set; }

        [MaxLength(50)]
        [Column("coa_id"), StringLength(50)]
        public string CoaId { get; set; }

        [MaxLength(80)]
        [Column(nameof(Name5)), StringLength(80)]
        public string Name5 { get; set; }

        [MaxLength(15)]
        [Column(nameof(Condition)), StringLength(15)]
        public string Condition { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column(nameof(Debit))]
        public decimal? Debit { get; set; }

        [Column(nameof(Credit))]
        public decimal? Credit { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("ap_level"), StringLength(1)]
        public string ApLevel { get; set; }

        [Required]
        [MaxLength(2)]
        [Column("line_code"), StringLength(2)]
        public string LineCode { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("d_k"), StringLength(1)]
        public string DK { get; set; }

        [MaxLength(50)]
        [Column("purchase_request_id"), StringLength(50)]
        public string PurchaseRequestId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("vendor_quotation_id"), StringLength(50)]
        public string VendorQuotationId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("company_brand_id"), StringLength(50)]
        public string CompanyBrandId { get; set; }

        [MaxLength(50)]
        [Column("tax_pay_coa_id"), StringLength(50)]
        public string TaxPayCoaId { get; set; }

        [Required]
        [Column("tax_payable_pct")]
        public decimal TaxPayablePct { get; set; }

        [MaxLength(50)]
        [Column("invoice_status_id"), StringLength(50)]
        public string InvoiceStatusId { get; set; }

        [MaxLength(50)]
        [Column("allocation_ratio_id"), StringLength(50)]
        public string AllocationRatioId { get; set; }
    }
}