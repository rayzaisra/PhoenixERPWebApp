using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetDepreciationYears
    {
        [DisplayName("Year To")]
        public int YearTo { get; set; }

        [DisplayName("First Period")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FirstPeriode { get; set; }

        [DisplayName("End Period")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndPeriode { get; set; }

        [DisplayName("Total month")]
        public int? TotalMonth { get; set; }

        [DisplayName("Initial Book Value")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? InitialBookValue { get; set; }

        [DisplayName("Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Rate { get; set; }

        [DisplayName("Depreciation Per Year")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DepreciationYear { get; set; }

        [DisplayName("Depreciation per month")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DepreciationMonth { get; set; }

        [DisplayName("End Book year")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? EndBookYear { get; set; }
    }
}
