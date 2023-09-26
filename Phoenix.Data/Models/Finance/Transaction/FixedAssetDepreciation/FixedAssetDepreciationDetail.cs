using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("fixed_asset_depreciation_detail", Schema = "fn")]
    public class FixedAssetDepreciationDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("jv_number")]
        public string JVNumber { get; set; }

        [Column("fixed_asset_id")]
        public string FixedAssetID { get; set; }

        [Column("fixed_asset_depreciation_id")]
        public string FixedAssetDepreciationId { get; set; }

        [Column("first_periode")]
        public DateTime? FirstPeriode { get; set; }

        [Column("book_value")]
        public decimal? BookValue { get; set; }

        [Column("depreciation_month")]
        public decimal? DepreciationMonth { get; set; }

        [Column("acquisition_amount")]
        public decimal? AcquisitionAmount { get; set; }

        [Column("accumulation_depreciation")]
        public decimal? AccumulationDepreciation { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [NotMapped]
        public string FixedAssetName { get; set; }

        [NotMapped]
        public string ItemCategoryName { get; set; }

        [NotMapped]
        public string LegalEntityName { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}
