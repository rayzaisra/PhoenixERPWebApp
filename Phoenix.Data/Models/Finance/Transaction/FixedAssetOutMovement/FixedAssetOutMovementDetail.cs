using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.FixedAssetOutMovement
{
    [Table("fixed_asset_out_detail", Schema = "fn")]
    public class FixedAssetOutMovementDetail : FinanceEntityBase
    {

        [Key]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("fixed_asset_out_id")]
        public string FixedAssetOutId { get; set; }
        [Column("fixed_asset_id")]
        public string FixedAssetId { get; set; }
        [Column("account_id")]
        public string AccountId { get; set; }
        [Column("sku_code")]
        public string SkuCode { get; set; }
        [Column("serial_number")]
        public string SerialNumber { get; set; }
        [Column("item_category_id")]
        public string ItemCategoryId { get; set; }
        [Column("item_category_name")]
        public string ItemCategoryName { get; set; }
        [Column("uom_id")]
        public string UomId { get; set; }
        [Column("unit_name")]
        public string UnitName { get; set; }
        [Column("brand")]
        public string Brand { get; set; }
        [Column("subbrand")]
        public string SubBrand { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("qty")]
        public decimal Qty { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [NotMapped]
        public string FileAttachment { get; set; }
        [Column("request_date")]
        public DateTime? RequestDate { get; set; }
        [NotMapped]
        public DateTime? RegistrationDate { get; set; }
        [Column("from_location_id")]
        public string LocationId { get; set; }
        [NotMapped]
        public string LocationName { get; set; }
        [Column("condition")]
        public string Condition { get; set; }
        [NotMapped]
        public string ConditionName { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [Column("url_link")]
        public string UrlLink { get; set; }
        [Column("request_status")]
        public string RequestStatus { get; set; }
        [NotMapped]
        public decimal QtyMax { get; set; }
        [NotMapped]
        public string RequestStatusName { get; set; }
    }
}













