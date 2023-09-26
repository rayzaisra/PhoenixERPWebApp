using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Media.MediaPlanEstimates
{
    public class MediaPlanDetail : MediaEntityBase
    {
        public string Id { get; set; }
        
        public string PlantHeaderID { get; set; }
        
        public string MarketId { get; set; }
        
        public string MediaTitleID { get; set; }
        
        public string MediaTypeID { get; set; }
        
        public decimal Calculation { get; set; }
        
        public string SpecPage { get; set; }
        
        public string SizeColumn { get; set; }
        
        public decimal SizeWidth { get; set; }
        
        public decimal SizeFcbw { get; set; }
        
        public string CurrencyID { get; set; }
        
        public decimal CurrencyRate { get; set; }
        
        public decimal Surcharge { get; set; }
        
        public decimal GrossAmount { get; set; }
        
        public decimal DiscPct { get; set; }
        
        public decimal NettAmount { get; set; }
        
        public decimal TotalInsertion { get; set; }
        
        public decimal TotalNettAmount { get; set; }
        
        public DateTime DatePlant { get; set; }
        
        public decimal ValueBonusAmount { get; set; }

        public int Revision { get; set; }

        public string FileName { get; set; }
        public bool? IsChecked { get; set; }

        public DateTime StartPeriode { get; set; }
        public DateTime EndPeriode { get; set; }
        public decimal ASFPercent { get; set; }
        public decimal ASFValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NettPaid { get; set; }
        public string FileMasterId { get; set; }
    }
}
