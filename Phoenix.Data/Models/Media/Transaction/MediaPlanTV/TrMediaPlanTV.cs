using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media.Transaction.MediaPlanTV
{
    [Table("TrMediaPlanTv", Schema = "media")]
    public class TrMediaPlanTV : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("media_plan_no"), StringLength(100)]
        public string MediaPlanNo { get; set; }

        [MaxLength(50)]
        [Column("version"), StringLength(50)]
        public string Version { get; set; }

        [MaxLength(50)]
        [Column("duration"), StringLength(50)]
        public string Duration { get; set; }

        [MaxLength(100)]
        [Column("target_audience"), StringLength(100)]
        public string TargetAudience { get; set; }

        [MaxLength(50)]
        [Column("media_type_id"), StringLength(50)]
        public string MediaTypeId { get; set; }

        [Column("period_campaign_start")]
        public DateTime? PeriodCampaignStart { get; set; }
        
        [Column("period_campaign_end")]
        public DateTime? PeriodCampaignEnd { get; set; }
        
        [Column("date_prepared")]
        public DateTime? DatePrepared { get; set; }

        [MaxLength(10)]
        [Column("revision_no"), StringLength(10)]
        public string RevisionNo { get; set; }

        [Column("total_net_cost")]
        public decimal? TotalNetCost { get; set; }

        [Column("agency_service_fee")]
        public decimal? AgencyServiceFee { get; set; }

        [Column("agency_service_fee_percent")]
        public decimal? AgencyServiceFeePercent { get; set; }

        [Column("tax_based")]
        public decimal? TaxBased { get; set; }

        [Column("vat")]
        public decimal? Vat { get; set; }

        [Column("grand_total")]
        public decimal? GrandTotal { get; set; }

        [MaxLength(50)]
        [Column("TmMediaPlanRequestType_id"), StringLength(50)]
        public string TmMediaPlanRequestTypeId { get; set; }

        [MaxLength(50)]
        [Column("TrMediaFileMaster_id"), StringLength(50)]
        public string TrMediaFileMasterId { get; set; }

        [MaxLength(50)]
        [Column("TmMediaPlanStatus_id"), StringLength(50)]
        public string TmMediaPlanStatusId { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(50)]
        [Column("brand_id"), StringLength(50)]
        public string BrandId { get; set; }

        [NotMapped]
        public string RequesterName { get; set; }

        [NotMapped]
        public string JobTitleName { get; set; }

        [NotMapped]
        public string MediaPlanStatusName { get; set; }
    }
}
