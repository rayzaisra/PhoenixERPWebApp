using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("invoice_receivable", Schema = "fn")]
    public class InvoiceReceivable : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("invoice_number"), StringLength(50)]
        public string InvoiceNumber { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(50)]
        [Column("po_id"), StringLength(50)]
        public string PoId { get; set; }

        [MaxLength(50)]
        [Column("invoice_vendor"), StringLength(50)]
        public string InvoiceVendor { get; set; }

        [Column("invoice_vendor_date")]
        public DateTime? InvoiceVendorDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("invoice_date")]
        public DateTime? InvoiceDate { get; set; }

        [Column("tax_date")]
        public DateTime? TaxDate { get; set; }

        [MaxLength(50)]
        [Column("tax_number"), StringLength(50)]
        public string TaxNumber { get; set; }

        [MaxLength(50)]
        [Column("good_received_id"), StringLength(50)]
        public string GoodReceivedId { get; set; }

        [MaxLength(150)]
        [Column(nameof(GoodReceivedNumber)), StringLength(150)]
        public string GoodReceivedNumber { get; set; }

        [MaxLength(50)]
        [Column("service_received_id"), StringLength(50)]
        public string ServiceReceivedId { get; set; }

        [MaxLength(150)]
        [Column("service_received_number"), StringLength(150)]
        public string ServiceReceivedNumber { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainServiceCategoryId { get; set; }

        [MaxLength(50)]
        [Column("share_services_id"), StringLength(50)]
        public string ShareServicesId { get; set; }

        [MaxLength(50)]
        [Column("legal_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(50)]
        [Column("allocation_ratio_id"), StringLength(50)]
        public string AllocationRatioId { get; set; }

        [Column("po_check_list")]
        public bool? PoCheckList { get; set; }

        [MaxLength(50)]
        [Column("po_object"), StringLength(50)]
        public string PoObject { get; set; }

        [Column("tax_check_list")]
        public bool? TaxCheckList { get; set; }

        [MaxLength(50)]
        [Column("tax_object"), StringLength(50)]
        public string TaxObject { get; set; }

        [Column("contract_check_list")]
        public bool? ContractCheckList { get; set; }

        [MaxLength(50)]
        [Column("contract_object"), StringLength(50)]
        public string ContractObject { get; set; }

        [Column("do_check_list")]
        public bool? DoCheckList { get; set; }

        [MaxLength(50)]
        [Column("do_object"), StringLength(50)]
        public string DoObject { get; set; }

        [Column("gr_check_list")]
        public bool? GrCheckList { get; set; }

        [MaxLength(50)]
        [Column("gr_object"), StringLength(50)]
        public string GrObject { get; set; }

        [Column("inv_check_list")]
        public bool? InvCheckList { get; set; }

        [MaxLength(50)]
        [Column("inv_object"), StringLength(50)]
        public string InvObject { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("payment_to")]
        public string PaymentTo { get; set; }

        [Column("NetAmount")]
        public decimal? NetAmount { get; set; }

        [Column("discount_percent")]
        public decimal? DiscountPercent { get; set; }

        [Column("discount_amount")]
        public decimal? DiscountAmount { get; set; }

        [Column("ValueAddedTaxPercent")]
        public decimal? ValueAddedTaxPercent { get; set; }

        [Column("ValueAddedTax")]
        public decimal? ValueAddedTax { get; set; }

        [Column(nameof(AdvertisingTax))]
        public decimal? AdvertisingTax { get; set; }

        [Column(nameof(TaxPayablePercent))]
        public decimal? TaxPayablePercent { get; set; }

        [Column(nameof(TaxPayable))]
        public decimal? TaxPayable { get; set; }

        [Column("total_cost_vendor")]
        public decimal? TotalCostVendor { get; set; }

        [Column("approval_front_desk")]
        public bool? ApprovalFrontDesk { get; set; }

        [Column("approval_admin_desk")]
        public bool? ApprovalAdminDesk { get; set; }

        [MaxLength(50)]
        [Column("invoice_status_id"), StringLength(50)]
        public string InvoiceStatusId { get; set; }

        [MaxLength(50)]
        [Column("media_order_number"), StringLength(50)]
        public string MediaOrderNumber { get; set; }

        [Column("tax_payable_coa"), StringLength(50)]
        public string TaxPayableCoaId { get; set; }

        [Column("remarks"), StringLength(500)]
        public string Remarks { get; set; }

        //For List Invoice Received & TAP
        [NotMapped]
        public string ClientName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string AccountPayableNumber { get; set; }


        //For Detail Form
        [NotMapped]
        public List<InvoiceReceivableDetail> InvoiceReceivableDetails { get; set; }
        [NotMapped]
        public PurchaseOrder PurchaseOrder { get; set; }
        [NotMapped]
        public Company Company { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public DateTime? TransactionDate { get; set; }

        //--------------Upload File---------
        [NotMapped]
        public IFormFile filePoObject { get; set; }
        [NotMapped]
        public string filePoObjectName { get; set; }
        [NotMapped]
        public IFormFile fileTaxObject { get; set; }
        [NotMapped]
        public string fileTaxObjectName { get; set; }
        [NotMapped]
        public IFormFile fileContractObject { get; set; }
        [NotMapped]
        public string fileContractObjectName { get; set; }
        [NotMapped]
        public IFormFile fileDoObject { get; set; }
        [NotMapped]
        public string fileDoObjectName { get; set; }
        [NotMapped]
        public IFormFile fileGrObject { get; set; }
        [NotMapped]
        public string fileGrObjectName { get; set; }
        [NotMapped]
        public IFormFile fileInvObject { get; set; }
        [NotMapped]
        public string fileInvObjectName { get; set; }

        //----------------------------------

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencyIdDefault { get; set; }

        [NotMapped]
        public string CurrencySymbolDefault { get; set; }
    }

    public class FilterInvoiceReceivable
    {
        public string Status { get; set; }
        public string BusinessUnitId { get; set; }
        public string LegalEntityId { get; set; }
        public string InvoiceReceivableId { get; set; }
        public string AccountPayableNumber { get; set; }
        public string VendorId { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string DateType { get; set; }
    }
}
