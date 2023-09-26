using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaPlanTV
{
    public class MediaPlanTVDetail : MediaEntityBase
    {
        public string Id { get; set; }
        public string TrMediaPlanId { get; set; }
        public string TmProgramCategoryTypeId { get; set; }
        public string TmMediaTypeId { get; set; }
        public string VendorId { get; set; }
        public string TmProgramPositionId { get; set; }
        public string TmProgramTypeId { get; set; }
        public string ProgramName { get; set; }
        public string DayStart { get; set; }
        public string DayEnd { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Duration { get; set; }
        public string Currency { get; set; }
        public decimal? BaseRate30 { get; set; }
        public decimal? UsedRate { get; set; }
        public decimal? SurCharge { get; set; }
        public decimal? GrossRate { get; set; }
        public decimal? Disc { get; set; }
        public decimal? Bvc { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? EstimateTvr { get; set; }
        public decimal? Tvr { get; set; }
        public decimal? Index { get; set; }
        public decimal? Cprp { get; set; }
        public decimal? TotalSpot { get; set; }
        public decimal? TotalTarps { get; set; }
        public decimal? GrossCommitedBonus { get; set; }
        public decimal? GrossTakenBonus { get; set; }
        public decimal? RemainingBonus { get; set; }
        public decimal? TotalGrossValue { get; set; }
        public decimal? TotalNetCost { get; set; }
        public string BroadcastTypeName { get; set; }
        public string PositionName { get; set; }
        public string TypeProgramName { get; set; }
    }
}
