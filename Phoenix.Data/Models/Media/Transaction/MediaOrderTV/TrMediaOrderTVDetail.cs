using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    [Table("TrMediaOrderTVDetail", Schema = "media")]
    public class TrMediaOrderTVDetail : MediaEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("TrMediaOrder_id"), StringLength(50)]
        public string TrMediaOrderId { get; set; }

        [MaxLength(100)]
        [Column("TmProgramCategoryType_id"), StringLength(50)]
        public string TmProgramCategoryTypeId { get; set; }

        [MaxLength(100)]
        [Column("TmMediaType_id"), StringLength(50)]
        public string TmMediaTypeId { get; set; }

        [MaxLength(100)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [MaxLength(100)]
        [Column("TmProgramPosition_id"), StringLength(50)]
        public string TmProgramPositionId { get; set; }

        [MaxLength(100)]
        [Column("TmProgramType_id"), StringLength(50)]
        public string ProgramTypeId { get; set; }
        
        [Column("program_name")]
        public string ProgramName { get; set; }

        [MaxLength(100)]
        [Column("day_start"), StringLength(100)]
        public string DayStart { get; set; }

        [MaxLength(100)]
        [Column("day_end"), StringLength(100)]
        public string DayEnd { get; set; }
        
        [Column("time_start")]
        public DateTime? TimeStart { get; set; }
        
        [Column("time_end")]
        public DateTime? TimeEnd { get; set; }

        [MaxLength(100)]
        [Column("duration"), StringLength(100)]
        public string Duration { get; set; }

        [MaxLength(100)]
        [Column("currency"), StringLength(100)]
        public string Currency { get; set; }
        
        [Column("base_rate_30")]
        public decimal? BaseRate30 { get; set; }
        
        [Column("used_rate")]
        public decimal? UsedRate { get; set; }
        
        [Column("surcharge")]
        public decimal? Surcharge { get; set; }
        
        [Column("gross_rate")]
        public decimal? GrossRate { get; set; }
        
        [Column("disc")]
        public decimal? Disc { get; set; }
        
        [Column("bvc")]
        public decimal? Bvc { get; set; }
        
        [Column("bonus")]
        public decimal? Bonus { get; set; }
        
        [Column("estimate_tvr")]
        public decimal? EstimateTvr { get; set; }
        
        [Column("tvr")]
        public decimal? Tvr { get; set; }
        
        [Column("index")]
        public decimal? Index { get; set; }
        
        [Column("cprp")]
        public decimal? Cprp { get; set; }
        
        [Column("total_spot")]
        public decimal? TotalSpot { get; set; }
        
        [Column("total_tarps")]
        public decimal? TotalTarps { get; set; }
        
        [Column("gross_comited_bonus")]
        public decimal? GrossCommitedBonus { get; set; }
        
        [Column("gross_taken_bonus")]
        public decimal? GrossTakenBonus { get; set; }
        
        [Column("remanining_bonus")]
        public decimal? RemainingBonus { get; set; }
        
        [Column("total_gross_value")]
        public decimal? TotalGrossValue { get; set; }
        
        [Column("total_net_cost")]
        public decimal? TotalNetCost { get; set; }

        //Custom
        [NotMapped]
        public string TrMediaPlanDetailId { get; set; }
    }
}
