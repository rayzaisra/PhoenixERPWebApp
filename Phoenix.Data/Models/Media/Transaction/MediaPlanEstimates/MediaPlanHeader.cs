using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Phoenix.Data.Models.Media
{
    [Table("plan_header", Schema = "media")]
    public class MediaPlanHeader : MediaPlanEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("plan_no"), StringLength(100)]
        public string PlanNo { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientID { get; set; }

        [MaxLength(50)]
        [Column("brand_id"), StringLength(50)]
        public string BrandID { get; set; }

        [MaxLength(100)]
        [Column("target_audience"), StringLength(100)]
        public string TargetAudience { get; set; }

        [MaxLength(50)]
        [Column("type_id"), StringLength(50)]
        public string TypeID { get; set; }

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
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityID { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyID { get; set; }


        [MaxLength(50)]
        [Column("tahun"), StringLength(50)]
        public string Tahun { get; set; }

        [MaxLength(50)]
        [Column("revisi"), StringLength(50)]
        public string Revisi { get; set; }

        [Column("campaign")]
        public bool Campaign { get; set; }

        [Column("exchange")]
        public decimal Exchange { get; set; }

        [MaxLength(450)]
        [Column("remark_rejected"), StringLength(450)]
        public string RemarkRejected { get; set; }

        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string DivisionId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Column("is_reported")]
        public bool? isReported { get; set; }

        [Column("report_file_id")]
        public string ReportFileId { get; set; }

        [NotMapped]
        public string ReportFileName { get; set; }


        [NotMapped]
        public string JobIdName { get; set; }

        [NotMapped]
        public string MediaTypeName { get; set; }

        [NotMapped]
        public string StatusName { get; set; }


        [NotMapped]
        public string ClientName { get; set; }

        [NotMapped]
        public string Brand { get; set; }

        [NotMapped]
        public decimal Gross { get; set; }

        [NotMapped]
        public decimal ASF { get; set; }
        [NotMapped]
        public int? ASFPercent { get; set; }

        [NotMapped]
        public string CheckedFlag { get; set; }

        [NotMapped]
        public List<MediaPlanDetail> MediaPlanDetails { get; set; }

        [NotMapped]
        public IFormFile reportfile { get; set; }
    }
}
