using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetDepreciationDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        public string JVNumber { get; set; }
        public string FixedAssetDepreciationId { get; set; }

        public string FixedAssetID { get; set; }
        
        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FirstPeriode { get; set; }

        [DisplayName("Book Value")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? BookValue { get; set; }

        [DisplayName("Deprecated per Month")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DepreciationMonth { get; set; }

        [DisplayName("Acquisition Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AcquisitionAmount { get; set; }

        [DisplayName("Accumulation Depreciation")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AccumulationDepreciation { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //NOT Mapped
        [DisplayName("Fixed Asset Name")]
        public string FixedAssetName { get; set; }

        [DisplayName("Category")]
        public string ItemCategoryName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
    }
}
