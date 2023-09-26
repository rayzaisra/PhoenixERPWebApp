using PhoenixERPWebApp.Models.Media.MediaPlanEstimates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaOrder
{
    public class MediaOrderHeader : MediaEntityBase
    {
        public string Id { get; set; }
        
        public string MediaPlanNo { get; set; }
        
        public string MediaOrderNumber { get; set; }
        
        public string ClientID { get; set; }
        
        public string BrandID { get; set; }
        
        public string TypeID { get; set; }
        
        public string Tahun { get; set; }
        
        public string Revisi { get; set; }
        
        public string StatusID { get; set; }
        
        public string JobID { get; set; }
        
        public string MainServiceCategoryID { get; set; }
        
        public string AffiliationID { get; set; }
        
        public string DivisionID { get; set; }
        
        public string LegalEntityID { get; set; }
        
        public string CurrencyID { get; set; }
        
        public bool Campaign { get; set; }
        
        public decimal? Exchange { get; set; }
        
        public DateTime? DateOrder { get; set; }// mo date
        
        public string VendorID { get; set; }
        
        public string VendorBrand { get; set; }
        
        public string PicName { get; set; }
        
        public string Address { get; set; }
        
        public string InvoicingType { get; set; }
        
        public string PaymentMethod { get; set; }
        
        public decimal? Surcharge { get; set; }
        
        public decimal? GrossAmount { get; set; }
        
        public decimal? DiscPercent { get; set; }
        
        public decimal? NettAmount { get; set; }
        
        public decimal? Term1Amount { get; set; }
        
        public decimal? Term2Amount { get; set; }
        
        public decimal? Term3Amount { get; set; }
        
        public decimal? Term1Pct { get; set; }
        
        public decimal? Term2Pct { get; set; }
        
        public decimal? Term3Pct { get; set; }
        
        public decimal? SubTotal { get; set; }
        
        public string ASFPercent { get; set; }

        public decimal? ASFValue { get; set; }
        
        public decimal? VATValue { get; set; }
        
        public decimal? GrandTotal { get; set; }
        
        public decimal? value_bonus_amount { get; set; }
        
        public string VendorName { get; set; }
        
        public string Remark { get; set; }

        
        public string NPWPNumber { get; set; }
        
        public string Description { get; set; }
        
        public string CommentHistory { get; set; }

        public string TypeName { get; set; }
        public string JobIDName { get; set; }

        public string StatusDesc { get; set; }
        public bool? isApprove { get; set; }

        public bool? isReported { get; set; }
        public string ReportFileId { get; set; }

        public string ReportFileName { get; set; }
        public string CheckedFlag { get; set; }
        public string RemarkRejected { get; set; }

        public List<MediaOrderDetail> MediaOrderDetails { get; set; }
        public MediaPlanHeader PlanHeaders { get; set; }
    }
}
