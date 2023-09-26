using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.InventoryIn
{
    [Table("inventory_in_detail", Schema ="fn")]
    public class InventoryInDetailModel : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("inventory_in_id")]
        public string InventoryInId { get; set; } //(nvarchar(50), null)
        [Column("item_id")]
        public string ItemId { get; set; } //(nvarchar(50), null)
        [Column("item_name")]
        public string ItemName { get; set; } //(nvarchar(450), null)
        [Column("item_category_id")]
        public string ItemCategoryId { get; set; } //(nvarchar(50), null)
        [Column("item_category_name")]
        public string ItemCategoryName { get; set; } //(nvarchar(450), null)
        [Column("uom_id")]
        public string UomId { get; set; } //(nvarchar(50), null)
        [Column("unit_name")]
        public string UnitName { get; set; } //(nvarchar(450), null)
        [Column("brand")]
        public string Brand { get; set; } //(nvarchar(450), null)
        [Column("subbrand")]
        public string Subbrand { get; set; } //(nvarchar(450), null)
        [Column("description")]
        public string Description { get; set; } //(nvarchar(450), null)
        [Column("qty")]
        public decimal Qty { get; set; } //(decimal(18,2), null)
        [Column("remark")]
        public string Remark { get; set; } //(nvarchar(450), null)
        [Column("file_attachment")]
        public string FileAttachment { get; set; } //(nvarchar(50), null)
    }
}