using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media
{
    public class MediaOrderTVDetail : MediaEntityBase
    {
        public string Id { get; set; }
        [DisplayName("MO Number")]
        public string MediaOrderNo { get; set; }
        [DisplayName("MO Date")]
        public DateTime? DatePrepared { get; set; }
        [DisplayName("Vendor")]
        public string BrandId { get; set; }
        [DisplayName("Client")]
        public string ClientId { get; set; }
        [DisplayName("Revision Number")]
        public string RevisionNo { get; set; }
        [DisplayName("Media Plan Number")]
        public string MediaPlanNo { get; set; }
        [DisplayName("Bulk Number")]
        public decimal? BulkNumber { get; set; }
        public bool IsTotal { get; set; }
        public decimal? Total { get; set; }
        [DisplayName("Gross Value")]
        public decimal? TotalGrossValue { get; set; }
        [DisplayName("Nett")]
        public decimal? TotalNetCost { get; set; }
        public decimal? Vat { get; set; }
        [DisplayName("Gross Paid")]
        public decimal? GrandTotal { get; set; }

        public string Description { get; set; }
        [DisplayName("Bulk Value")]
        public decimal? BulkValue { get; set; }
        [DisplayName("Total Insertion")]
        public decimal? TotalInsertion { get; set; }

        public string TrMediaPlanTvId { get; set; }
        public string TrMediaFileMasterId { get; set; }

        [DisplayName("Order Status")]
        public string TmMediaOrderStatusId { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public string Version { get; set; }
        public string Duration { get; set; }
        public string TargetAudience { get; set; }
        public string PeriodCampaignStart { get; set; }
        public string PeriodCampaignEnd { get; set; }
        public decimal? AgencyServiceFee { get; set; }
        public decimal? AgencyServiceFeePercent { get; set; }
        public decimal? TaxBased { get; set; }
        
    }
}
