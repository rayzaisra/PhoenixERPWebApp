using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetDepreciation : FinanceEntityBase
    {
        public string Id { get; set; }

        public string FixedAssetID { get; set; }

        [DisplayName("Acquisition Cost")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AcquisitionAmount { get; set; }

        [DisplayName("Life Time (Month)")]
        public int? LifeTime { get; set; }

        [DisplayName("Residue")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Residual { get; set; }

        [DisplayName("Depreciation Value")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DepreciationValue { get; set; }

        [DisplayName("Set Depreciation")]
        public string AccountSetDepreceationId { get; set; }

        [DisplayName("Set Accumulation Depreciation")]
        public string AccountSetAccumulationDepreceationId { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Accumulation Depreciation")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AccumulationDepreciation { get; set; }

        [DisplayName("Book Value")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? BookValue { get; set; }

        [DisplayName("Acquisiton Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AcquisitonDate { get; set; }

        [NotMapped]
        public FixedAsset FixedAsset { get; set; }
    }
}
