using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    [Table("TrMediaOrderTV", Schema = "media")]
    public class TrMediaOrderTV : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        
        [MaxLength(100)]
        [Column("Media_Order_No"), StringLength(100)]
        public string MediaOrderNo { get; set; }
        
        [MaxLength(50)]
        [Column("Version"), StringLength(50)]
        public string Version { get; set; }
        
        [MaxLength(50)]
        [Column("Duration"), StringLength(50)]
        public string Duration { get; set; }
        
        [MaxLength(100)]
        [Column("Target_Audience"), StringLength(100)]
        public string TargetAudience { get; set; }
        
        [Column("Period_Campaign_Start")]
        public DateTime? PeriodCampaignStart { get; set; }
        
        [Column("Period_Campaign_End")]
        public DateTime? PeriodCampaignEnd { get; set; }
        
        [Column("Date_Prepared")]
        public DateTime? DatePrepared { get; set; }
        
        [MaxLength(50)]
        [Column("Revision_No"), StringLength(50)]
        public string RevisionNo { get; set; }
        
        [Column("Total_Net_Cost")]
        public decimal? TotalNetCost { get; set; }

        [Column("Total_Gross_Value")]
        public decimal? TotalGrossValue { get; set; }

        [Column("Agency_Service_Fee"),]
        public decimal? AgencyServiceFee { get; set; }
        
        [Column("Agency_Service_Fee_Percent")]
        public decimal? AgencyServiceFeePercent { get; set; }
        
        [Column("Tax_Based")]
        public decimal? TaxBased { get; set; }
        
        [Column("Vat")]
        public decimal? Vat { get; set; }
        
        [Column("Grand_Total")]
        public decimal? GrandTotal { get; set; }
        
        [MaxLength(50)]
        [Column("TrMediaFileMaster_Id"), StringLength(50)]
        public string TrMediaFileMasterId { get; set; }
        
        [MaxLength(50)]
        [Column("TmMediaOrderStatus_Id"), StringLength(50)]
        public string TmMediaOrderStatusId { get; set; }
        
        [MaxLength(50)]
        [Column("Employee_Basic_Info_Id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }
        
        [Column("Bulk_Value")]
        public decimal? BulkValue { get; set; }
        
        [Column("Total_Insertion")]
        public decimal? TotalInsertion { get; set; }
        
        [MaxLength(50)]
        [Column("TrMediaPlanTv_Id"), StringLength(50)]
        public string TrMediaPlanTvId { get; set; }

        [MaxLength(50)]
        [Column("client_id"), StringLength(50)]
        public string ClientId { get; set; }

        [MaxLength(50)]
        [Column("brand_id"), StringLength(50)]
        public string BrandId { get; set; }
    }
}
