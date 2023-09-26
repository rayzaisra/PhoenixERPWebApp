using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    public class FixedAssetDepreciationYears
    {
        public int YearTo { get; set; }
        public DateTime? FirstPeriode { get; set; }
        public DateTime? EndPeriode { get; set; }
        public int? TotalMonth { get; set; }
        public decimal? InitialBookValue { get; set; }
        public decimal? Rate { get; set; }
        public decimal? DepreciationYear { get; set; }
        public decimal? DepreciationMonth { get; set; }
        public decimal? EndBookYear { get; set; }
    }
}
