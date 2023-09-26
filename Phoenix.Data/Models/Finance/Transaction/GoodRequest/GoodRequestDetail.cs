using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance
{
    [Table("inventory_goods_request_detail", Schema="fn")]
    public class GoodRequestDetail : FinanceEntityBase 
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("inventory_goods_request_id")]
        public string GoodRequestId { get; set; }
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
        public string UomName { get; set; }
        [Column("brand")]
        public string Brand { get; set; }
        [Column("subbrand")]
        public string Subbrand { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("qty")]
        public decimal? qty { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("file_attachment")]
        public string FileAttachment { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("warehouse_id_from")]
        public string WarehouseIdFrom { get; set; }
        [Column("warehouse_id_to")]
        public string WarehouseIdTo { get; set; }
        [Column("purpose")]
        public string Purpose { get; set; }
        [Column("inv_fa_detail_id")]
        public string InvFaDetailId { get; set; }
        [Column("location")]
        public string Location { get; set; }
        [Column("location_name")]
        public string LocationName { get; set; }
        [Column("total_used_days")]
        public decimal? TotalUsedDays { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Return_detail { get; set; }
        [NotMapped]
        public decimal qtyMax { get; set; }
    }
}
