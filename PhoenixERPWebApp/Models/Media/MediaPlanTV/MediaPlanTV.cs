using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaPlanTV
{
    public class MediaPlanTV : MediaEntityBase
    {
        [DisplayName("Request Id")]
        public string Id { get; set; }

        public string MediaPlanNo { get; set; }
        
        public string Version { get; set; }
        
        public string Duration { get; set; }
        
        public string TargetAudience { get; set; }
        
        public string MediaTypeId { get; set; }
        
        public DateTime? PeriodCampaignStart { get; set; }
        
        public DateTime? PeriodCampaignEnd { get; set; }
        
        public DateTime? DatePrepared { get; set; }
        
        public string RevisionNo { get; set; }
        
        [DisplayName("Total Net Cost")]
        public double? TotalNetCost { get; set; }
        
        [DisplayName("Agency Service Fee")]
        public double? AgencyServiceFee { get; set; }
        
        public double? AgencyServiceFeePercent { get; set; }
        
        public double? TaxBased { get; set; }
        
        public double? Vat { get; set; }

        [DisplayName("Grand Total")]
        public double? GrandTotal { get; set; }
        
        public string TmMediaPlanRequestTypeId { get; set; }
        
        public string TrMediaFileMasterId { get; set; }

        public string FileMasterName { get; set; }
        
        public string TmMediaPlanStatusId { get; set; }
        
        public string EmployeeBasicInfoId { get; set; }
        
        public string ClientId { get; set; }
        
        public string BrandId { get; set; }
        
        public string BusinessUnitDivisionId { get; set; }
        
        public string BusinessUnitDepartementId { get; set; }

        [DisplayName("Request By")]
        public string RequesterName { get; set; }

        [DisplayName("Job Title")]
        public string JobTitleName { get; set; }

        [DisplayName("Status")]
        public string MediaPlanStatusName { get; set; }
        public string MediaPlanTvFileTemplateId { get; set; }
    }
}
