using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.Transaction.MediaOrder
{
    [Table("order", Schema = "media")]
    public class MediaOrderHeader : MediaPlanEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("mediaplan_number"), StringLength(100)]
        public string MediaPlanNo { get; set; }

        [Column("order_no"), StringLength(100)]
        public string MediaOrderNumber { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientID { get; set; }

        [MaxLength(50)]
        [Column("brand_id"), StringLength(50)]
        public string BrandID { get; set; }

        [MaxLength(50)]
        [Column("type_id"), StringLength(50)]
        public string TypeID { get; set; }

        [MaxLength(50)]
        [Column("tahun"), StringLength(50)]
        public string Tahun { get; set; }
        
        [MaxLength(50)]
        [Column("revision"), StringLength(50)]
        public string Revisi { get; set; }
        
        [MaxLength(50)]
        [Column("status_id"), StringLength(50)]
        public string StatusID { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobID { get; set; }       

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainServiceCategoryID { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationID { get; set; }
        
        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string DivisionID { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityID { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyID { get; set; }
        
        [Column("campaign_yes")]
        public bool Campaign { get; set; }

        [Column("exchange")]
        public decimal? Exchange { get; set; }

        [Column("mo_date")]
        public DateTime? DateOrder { get; set; }// mo date

        [Column("vendor_id"), StringLength(50)]
        public string VendorID { get; set; }

        [Column("vendor_brand"), StringLength(50)]
        public string VendorBrand { get; set; }

        [Column("pic_name"), StringLength(50)]
        public string PicName { get; set; }

        [Column("address_vendor"), StringLength(50)]
        public string Address { get; set; }

        [Column("invoicing_type"), StringLength(50)]
        public string InvoicingType { get; set; }

        [Column("payment_method_id"), StringLength(50)]
        public string PaymentMethod { get; set; }

        [Column("surcharge")]
        public decimal? Surcharge { get; set; }

        [Column("gross_amount")]
        public decimal? GrossAmount { get; set; }

        [Column("disc_pct")]
        public decimal? DiscPercent { get; set; }

        [Column("nett_amount")]
        public decimal? NettAmount { get; set; }

        [Column("term_1")]
        public decimal? Term1Amount { get; set; }

        [Column("term_2")]
        public decimal? Term2Amount { get; set; }

        [Column("term_3")]
        public decimal? Term3Amount { get; set; }

        [Column("term_1_pct")]
        public decimal? Term1Pct { get; set; }

        [Column("term_2_pct")]
        public decimal? Term2Pct { get; set; }

        [Column("term_3_pct")]
        public decimal? Term3Pct { get; set; }

        [Column("sub_total")]
        public decimal? SubTotal { get; set; }

        [Column("asf_percent")]
        public string ASFPercent { get; set; }

        [Column("asf_value")]
        public decimal? ASFValue { get; set; }

        [Column("vat")]
        public decimal? VATValue { get; set; }

        [Column("grand_total")]
        public decimal? GrandTotal { get; set; }

        [Column("value_bonus_amount")]
        public decimal? value_bonus_amount { get; set; }

        [Column("vendor_name"), StringLength(50)]
        public string VendorName { get; set; }

        [Column("remarks"), StringLength(50)]
        public string Remark { get; set; }
        
        [Column("npwp"), StringLength(50)]
        public string NPWPNumber { get; set; }

        [Column("description"), StringLength(50)]
        public string Description { get; set; }

        [Column("comment_history"), StringLength(50)]
        public string CommentHistory { get; set; }

        [Column("is_reported")]
        public bool? isReported { get; set; }

        [Column("report_file_id")]
        public string ReportFileId { get; set; }

        [NotMapped]
        public string CheckedFlag { get; set; }

        [NotMapped]
        public string ReportFileName { get; set; }
        
        [NotMapped]
        public string TypeName { get; set; }
        [NotMapped]
        public string JobIDName { get; set; }
      
        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public int? TermOfPayment { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

        [NotMapped]
        public List<MediaOrderDetail> MediaOrderDetails { get; set; }
        [NotMapped]
        public IFormFile reportfile { get; set; }
    }
}
