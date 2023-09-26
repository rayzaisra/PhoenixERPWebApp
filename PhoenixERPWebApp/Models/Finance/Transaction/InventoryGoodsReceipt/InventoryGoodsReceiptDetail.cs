using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class InventoryGoodsReceiptDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Purchase Order Detail Id")]
        public string PurchaseOrderDetailId { get; set; }

        [DisplayName("Inventory Goods Receipt Id")]
        public string InventoryGoodsReceiptId { get; set; }

        [DisplayName("Task Id")]
        public string TaskId { get; set; }

        [DisplayName("Task")]
        public string Task { get; set; }

        [DisplayName("Item Type Id")]
        public string ItemTypeId { get; set; }

        [DisplayName("Item Type Name")]
        public string ItemTypeName { get; set; }

        [DisplayName("Item Id")]
        public string ItemId { get; set; }

        [DisplayName("Item Code")]
        public string ItemCode { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        [DisplayName("Inventory Id")]
        public string InventoryId { get; set; }

        [DisplayName("Inventory Name")]
        public string InventoryName { get; set; }

        [DisplayName("Uom Id")]
        public string UomId { get; set; }

        [DisplayName("Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Received Qty")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ReceivedQty { get; set; }

        [DisplayName("Purchased Qty")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PurchasedQty { get; set; }

        //[DisplayName("Total Received Qty")]
        //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        //public decimal? TotalReceivedQty { get; set; }

        [DisplayName("Remaining Qty")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? RemainingQty { get; set; }


        //[DisplayName("Item Category Id")]
        //public string ItemCategoryId { get; set; }

        //[DisplayName("Item Category Name")]
        //public string ItemCategoryName { get; set; }

        //[DisplayName("Brand")]
        //public string Brand { get; set; }

        //[DisplayName("Subbrand")]
        //public string Subbrand { get; set; }

        //[DisplayName("Description")]
        //public string Description { get; set; }

        //[DisplayName("Remark")]
        //public string Remark { get; set; }

        //[DisplayName("File Attachment")]
        //public string FileAttachment { get; set; }

        //[NotMapped]
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public string Location { get; set; }

        // buat perhitungan remaining
        public decimal? LastRemainingQty { get; set; }
    }
}