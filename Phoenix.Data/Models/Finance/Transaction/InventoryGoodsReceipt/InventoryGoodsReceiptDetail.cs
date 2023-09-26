using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("inventory_goods_receipt_detail", Schema = "fn")]
    public class InventoryGoodsReceiptDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_detail_id"), StringLength(50)]
        public string PurchaseOrderDetailId { get; set; }

        [MaxLength(50)]
        [Column("inventory_goods_receipt_id"), StringLength(50)]
        public string InventoryGoodsReceiptId { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [MaxLength(100)]
        [Column(nameof(Task)), StringLength(100)]
        public string Task { get; set; }

        [MaxLength(50)]
        [Column("item_type_id"), StringLength(50)]
        public string ItemTypeId { get; set; }

        [MaxLength(450)]
        [Column("item_type_name"), StringLength(450)]
        public string ItemTypeName { get; set; }

        [MaxLength(50)]
        [Column("item_id"), StringLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        [Column("item_code"), StringLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(450)]
        [Column("item_name"), StringLength(450)]
        public string ItemName { get; set; }

        [MaxLength(50)]
        [Column("inventory_id"), StringLength(50)]
        public string InventoryId { get; set; }

        [MaxLength(100)]
        [Column("inventory_name"), StringLength(100)]
        public string InventoryName { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [MaxLength(450)]
        [Column("unit_name"), StringLength(450)]
        public string UnitName { get; set; }


        [Column("received_qty")]
        public decimal? ReceivedQty { get; set; }

        [Column("purchased_qty")]
        public decimal? PurchasedQty { get; set; }

        //[Column("total_received_qty")]
        //public decimal? TotalReceivedQty { get; set; }

        [Column("remaining_qty")]
        public decimal? RemainingQty { get; set; }

        //[MaxLength(50)]
        //[Column("item_category_id"), StringLength(50)]
        //public string ItemCategoryId { get; set; }

        //[MaxLength(450)]
        //[Column("item_category_name"), StringLength(450)]
        //public string ItemCategoryName { get; set; }

        //[MaxLength(450)]
        //[Column(nameof(Brand)), StringLength(450)]
        //public string Brand { get; set; }

        //[MaxLength(450)]
        //[Column(nameof(Subbrand)), StringLength(450)]
        //public string Subbrand { get; set; }

        //[MaxLength(450)]
        //[Column(nameof(Description)), StringLength(450)]
        //public string Description { get; set; }

        //[MaxLength(450)]
        //[Column(nameof(Remark)), StringLength(450)]
        //public string Remark { get; set; }

        //[MaxLength(50)]
        //[Column("file_attachment"), StringLength(50)]
        //public string FileAttachment { get; set; }

        [NotMapped]
        public decimal? UnitPrice { get; set; }
        [NotMapped]
        public decimal? Amount { get; set; }
        [NotMapped]
        public string Location { get; set; }
        [NotMapped]
        public decimal? LastRemainingQty { get; set; }
    }
}