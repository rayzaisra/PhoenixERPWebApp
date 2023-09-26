using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("request_for_quotation_vendor", Schema = "fn")]
    public class RequestForQuotationVendor : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("request_for_quotation_id"), StringLength(50)]
        public string RequestForQuotationId { get; set; }

        [MaxLength(50)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [Column("sub_total_vendor")]
        public decimal? SubTotalVendor { get; set; }

        [Column("vat_percent")]
        public decimal? VatPercent { get; set; }

        [Column(nameof(Vat))]
        public decimal? Vat { get; set; }

        [Column("total_amount_vendor")]
        public decimal? TotalAmountVendor { get; set; }

        [Column(nameof(Description)), StringLength(250)]
        public string Description { get; set; }

        [Column("is_request_purchase_order")]
        public bool? IsRequestPurchaseOrder { get; set; }

        [MaxLength(50)]
        [Column("filemaster_id"), StringLength(50)]
        public string FilemasterId { get; set; }

        //[MaxLength(450)]
        //[Column("email_recipient"), StringLength(450)]
        //public string EmailRecipient { get; set; }

        //[MaxLength(450)]
        //[Column("email_subject"), StringLength(450)]
        //public string EmailSubject { get; set; }

        //[MaxLength(450)]
        //[Column("email_bcc"), StringLength(450)]
        //public string EmailBcc { get; set; }

        //[MaxLength(450)]
        //[Column("email_cc"), StringLength(450)]
        //public string EmailCc { get; set; }

        //[MaxLength(450)]
        //[Column("email_attachment"), StringLength(450)]
        //public string EmailAttachment { get; set; }

        [NotMapped]
        public string VendorName { get; set; }

        [NotMapped]
        public string FilemasterName { get; set; }
        
        [NotMapped]
        public string CurrencyRequestId { get; set; }

        [NotMapped]
        public decimal? ExchangeRateRequest { get; set; }

        [NotMapped]
        public string OperatorRequest { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }
    }
}