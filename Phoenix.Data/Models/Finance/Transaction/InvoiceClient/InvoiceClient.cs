using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("invoice_client", Schema = "fn")]
    public class InvoiceClient : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pce_id"), StringLength(50)]
        public string PceId { get; set; }

        [Column("pce_seq")]
        public int? PceSeq { get; set; }

        [Column("brand_id")]
        public string BrandId { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(50)]
        [Column("alternate_client_id"), StringLength(50)]
        public string AlternateClientId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("invoice_date")]
        public DateTime? InvoiceDate { get; set; }

        [MaxLength(50)]
        [Column("invoice_number"), StringLength(50)]
        public string InvoiceNumber { get; set; }

        [Column("start_period")]
        public DateTime? StartPeriod { get; set; }

        [Column("end_period")]
        public DateTime? EndPeriod { get; set; }

        [Column("vat_no")]
        public string VatNo { get; set; }

        //[Required]
        //[Column("due_date")]
        //public DateTime DueDate { get; set; }

        [Column(nameof(Vat))]
        public decimal? Vat { get; set; }

        [Column("vat_exchange_rate")]
        public decimal? VatExchangeRate { get; set; }

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(255)]
        [Column("job_name"), StringLength(255)]
        public string JobName { get; set; }

        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(500)]
        [Column(nameof(Remarks)), StringLength(500)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        [Column("estimate_number"), StringLength(50)]
        public string EstimateNumber { get; set; }

        [MaxLength(250)]
        [Column("ref_number"), StringLength(250)]
        public string RefNumber { get; set; }

        [MaxLength(250)]
        [Column("your_ref"), StringLength(250)]
        public string YourRef { get; set; }

        [Column(nameof(Status))]
        public string Status { get; set; }

        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [Column("asf")]
        public decimal? Asf { get; set; }

        [Column("asf_amount")]
        public decimal? AsfAmount { get; set; }

        [Column("grand_total")]
        public decimal? GrandTotal { get; set; }

        [Column("multi_pce_id")]
        public string MultiPceId { get; set; }

        [Column("pay_description"), StringLength(500)]
        public string PayDescription { get; set; }

        [Column("po_doc_file_id")]
        public string PoDocFileId { get; set; }

        [Column("vat_doc_file_id")]
        public string VatDocFileId { get; set; }

        [Column("main_service_category")]
        public string MainServiceCategory { get; set; }

        [Column("legal_entity_id")]
        public string LegalId { get; set; }

        [Column("affiliation_id")]
        public string AffiliationId { get; set; }

        [Column("business_unit_division_id")]
        public string BusinessUnitId { get; set; }

        [Column("po_client_no")]
        public string POClientNo { get; set; }

        [Column("invoice_billed")]
        public decimal? InvoiceBilled { get; set; }

        [Column("pce_type_crossbilling")]
        public bool? PceTypeCrossBilling { get; set; }

        [NotMapped]
        public IFormFile PoDocFile { get; set; }
        [NotMapped]
        public IFormFile VatDocFile { get; set; }

        [NotMapped]
        public string PoDocFileName { get; set; }
        [NotMapped]
        public string VatDocFileName { get; set; }


        //For List Invoice Client
        [NotMapped]
        public string ClientName { get; set; }

        [NotMapped]
        public string AccountBankName { get; set; }

        [NotMapped]
        public string AccountBankNumber { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string DivisionName { get; set; }

        [NotMapped]
        public string MainServiceCategoryName { get; set; }

        [NotMapped]
        public string CurrencyName { get; set; }

        //For Detail Invoice Client - Process Crud
        [NotMapped]
        public List<InvoiceClientItemOther> items_other { get; set; }


        //======== Start Detail Invoice Client ============
        //For Approval 
        [NotMapped]
        public string LogDescription { get; set; }


        //For Client Information
        [NotMapped]
        public string ClientBrandName { get; set; }

        [NotMapped]
        public string ClientInvAddress { get; set; }

        [NotMapped]
        public string ClientTaxNumber { get; set; }

        [NotMapped]
        public string ClientPIC { get; set; }

        [NotMapped]
        public int? TermOfPayment { get; set; }


        //For Invoice Detail
        [NotMapped]
        public decimal? TotalPCE { get; set; }

        [NotMapped]
        public decimal? totalamtplusasf { get; set; }

        [NotMapped]
        public decimal? PceBalance { get; set; }

        [NotMapped]
        public decimal? VAtPercentage { get; set; }

        [NotMapped]
        public string InvoiceTo { get; set; }


        //For Job Detail 

        [NotMapped]
        public string JobStatus { get; set; }
        //======== End Detail Invoice Client ============ 


        //For AR 
        [NotMapped]
        public DateTime? InvoiceDeliveryDate { get; set; }

        [NotMapped]
        public string InvoiceDeliveryNo { get; set; }

        //For Multiple Invoice Client
        //[NotMapped]
        //public decimal? BilledAsfPercent { get; set; }
        [NotMapped]
        public List<InvoiceClient> PceListMultiple { get; set; }

        [NotMapped]
        public List<InvoiceClientItemOther> InvoiceMultipleListOther { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencyIdDefault { get; set; }

        [NotMapped]
        public string CurrencySymbolDefault { get; set; }

        [NotMapped]
        public List<MultipleBrand> MultipleBrand { get; set; }

        [NotMapped]
        public string BankReceivedNumber { get; set; }

        [NotMapped]
        public decimal? BankReceivedAmount { get; set; }

        [NotMapped]
        public string BankReceivedStatus { get; set; }

        [NotMapped]
        public string BankReceivedCurrencySymbol { get; set; }

    }

    public class MultipleBrand
    {
        public string BrandId { get; set; }
        public string ClientBrandName { get; set; }
        public string ClientInvAddress { get; set; }
        public string ClientPIC { get; set; }
        public int? TermOfPayment { get; set; }
    }

    public class FilterInvoiceClient
    { 
        public string Status { get; set; }

        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }

        [DisplayName("Date")]
        public string DateType { get; set; }
    }
}