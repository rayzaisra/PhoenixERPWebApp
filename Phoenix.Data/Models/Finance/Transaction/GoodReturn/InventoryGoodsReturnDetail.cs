using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.GoodReturn
{
    [Table("inventory_goods_return_detail", Schema = "fn")]
    public class InventoryGoodsReturnDetail: FinanceEntityBase
    {

        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("inventory_goods_return_id")]
        public string InventoryGoodsReturnId { get; set; }

        [Column("item_id")]
        public string ItemId { get; set; }

        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("task_id")]
        public string TaskId { get; set; }

        [Column("task_detail")]
        public string TaskDetail { get; set; }

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
        public decimal? Qty { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("file_attachment")]
        public string FileAttachment { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("warehouse_id_from")]
        public string WareHouseIdFrom { get; set; }
        [Column("warehouse_id_to")]
        public string WareHouseIdTo { get; set; }
        [Column("purpose")]
        public string Purpose { get; set; }
        [Column("inv_fa_detail_id")]
        public string InvFaDetailId { get; set; }
        [Column("location")]
        public string Location { get; set; }
        [Column("location_name")]
        public string LocationName { get; set; }

    }
}
