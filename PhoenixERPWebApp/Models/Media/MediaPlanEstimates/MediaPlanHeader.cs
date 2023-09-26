using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaPlanEstimates
{
    public class MediaPlanHeader : MediaEntityBase  
    {
        public string Id { get; set; }
        [DisplayName("MP Number")]
        public string PlanNo { get; set; }
        public string ClientID { get; set; }
        public string BrandID { get; set; }
        public string TargetAudience { get; set; }
        public string TypeID { get; set; }
        public string Tahun { get; set; }
        public string Revisi { get; set; }
        [DisplayName("Job ID - Name")]
        public string JobIdName { get; set; }
        [DisplayName("Media Type")]
        public string MediaTypeName { get; set; }
        [DisplayName("Campaign")]
        public bool Campaign { get; set; }
        [DisplayName("Client Name")]
        public string ClientName { get; set; }
        [DisplayName("Brand")]
        public string Brand { get; set; }
        [DisplayName("Gross")]
        public decimal Gross { get; set; }
        [DisplayName("ASF")]
        public decimal ASF { get; set; }
        public int? ASFPercent { get; set; }
        public string StatusID { get; set; }
        public string StatusName { get; set; }

        public string JobID { get; set; }

        public string ReportFileId { get; set; }
        
        public string ReportFileName { get; set; }

        public string MainServiceCategoryID { get; set; }
        
        //public string DivisionID { get; set; }
        public string AffiliationID { get; set; }
        public string LegalEntityID { get; set; }
        
        public string CurrencyID { get; set; }
        public string CampaignValue { get; set; }

        public  decimal  Exchange  { get; set; }
        public string RemarkRejected { get; set; }
        public string CheckedFlag { get; set; }
        public bool? isApprove { get; set; }
        public string BusinessUnitId { get; set; }
        public bool? isReported { get; set; }
        public List<MediaPlanDetail> MediaPlanDetails { get; set; }

    }
}
