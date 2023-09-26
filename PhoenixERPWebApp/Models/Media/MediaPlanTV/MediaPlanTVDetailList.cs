using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaPlanTV
{
    public class MediaPlanTVDetailList : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("Media Plan No")]
        public string TrMediaPlanId { get; set; }
        [DisplayName("Category Type")]
        public string TmProgramCategoryTypeId { get; set; }
        [DisplayName("Media Type")]
        public string TmMediaTypeId { get; set; }
        [DisplayName("Vendor")]
        public string VendorId { get; set; }
        [DisplayName("Program Position")]
        public string TmProgramPositionId { get; set; }
        [DisplayName("Program Type")]
        public string TmProgramTypeId { get; set; }
        [DisplayName("Program Name")]
        public string ProgramName { get; set; }
        [DisplayName("Day Start")]
        public string DayStart { get; set; }
        [DisplayName("Day End")]
        public string DayEnd { get; set; }
        [DisplayName("Time Start")]
        public DateTime? TimeStart { get; set; }
        [DisplayName("Time End")]
        public DateTime? TimeEnd { get; set; }
        public string Duration { get; set; }
        public string Currency { get; set; }
        [DisplayName("Base Rate 30")]
        public decimal? BaseRate30 { get; set; }
        [DisplayName("Used Rate")]
        public decimal? UsedRate { get; set; }
        public decimal? SurCharge { get; set; }
        [DisplayName("Gross Rate")]
        public decimal? GrossRate { get; set; }
        [DisplayName("Discount")]
        public decimal? Disc { get; set; }
        public decimal? Bvc { get; set; }
        public decimal? Bonus { get; set; }
        [DisplayName("Estimate Tvr")]
        public decimal? EstimateTvr { get; set; }
        public decimal? Tvr { get; set; }
        public decimal? Index { get; set; }
        public decimal? Cprp { get; set; }
        [DisplayName("Total Spot")]
        public decimal? TotalSpot { get; set; }
        [DisplayName("Total Tarps")]
        public decimal? TotalTarps { get; set; }
        [DisplayName("Gross Commited Bonus")]
        public decimal? GrossCommitedBonus { get; set; }
        [DisplayName("Gross Taken Bonus")]
        public decimal? GrossTakenBonus { get; set; }
        [DisplayName("Remaining Bonus")]
        public decimal? RemainingBonus { get; set; }
        [DisplayName("Total Gross Value")]
        public decimal? TotalGrossValue { get; set; }
        [DisplayName("Total Nett Cost")]
        public decimal? TotalNetCost { get; set; }
        [DisplayName("Net Rate")]
        public decimal? NetRate { get; set; }
        public decimal? ProRate { get; set; }
        public DateTime? DetailPlanDate { get; set; }
        public string BroadcastTypeName { get; set; }
        public string TypeProgramName { get; set; }

        public string TrMediaPlanDetailId
        {
            get
            {
                return this.Id;
            }
        }
    }
}
