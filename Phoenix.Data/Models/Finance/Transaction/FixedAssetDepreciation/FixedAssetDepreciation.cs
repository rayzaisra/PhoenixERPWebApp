using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("fixed_asset_depreciation", Schema = "fn")]
    public class FixedAssetDepreciation : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("fixed_asset_id")]
        public string FixedAssetID { get; set; }

        [Column("acquisition_amount")]
        public decimal? AcquisitionAmount { get; set; }

        [Column("life_time")]
        public int? LifeTime { get; set; }

        [Column("residual")]
        public decimal? Residual { get; set; }

        [Column("depreceation_value")]
        public decimal? DepreciationValue { get; set; }

        [Column("acc_set_dep_id")]
        public string AccountSetDepreceationId { get; set; }

        [Column("acc_set_accumulation_dep_id")]
        public string AccountSetAccumulationDepreceationId { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [NotMapped]
        public decimal? AccumulationDepreciation { get; set; }

        [NotMapped]
        public decimal? BookValue { get; set; }

        [NotMapped]
        public DateTime? AcquisitonDate { get; set; }

        [NotMapped]
        public FixedAsset FixedAsset { get; set; }         
    }
}
