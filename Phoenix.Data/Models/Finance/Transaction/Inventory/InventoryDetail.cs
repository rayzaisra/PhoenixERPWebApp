using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("inventory_detail", Schema = "fn")]
    public class InventoryDetail : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("inventory_id")]
        public string InventoryId { get; set; } //(nvarchar(50), null)
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

        [Column("good_receipt_detail_id")]
        public string GoodReceiptDetailId { get; set; }
        [Column("good_receipt_id")]
        public string GoodReceiptId { get; set; }
        [Column("Date_Receipt")]
        public DateTime? DateReceipt { get; set; }
        [Column("Manufacturer")]
        public string Manufacturer { get; set; }
        [Column("Model")]
        public string Model { get; set; }
        [Column("Product_Number")]
        public string ProductNumber { get; set; }
        [Column("Color")]
        public string Color { get; set; }
        [Column("Condition")]
        public string Condition { get; set; }
        [Column("sku_code")]
        public string SkuCode { get; set; }
        [Column("Location")]
        public string Location { get; set; }

        [Column("status_item")]
        public string Status { get; set; }

        [Column("vendor_name_id")]
        public string VendorNameId { get; set; }

        [Column("vendor_brand_id")]
        public string VendorBrandId { get; set; }

        [NotMapped]
        public string status_name { get; set; }

        [Column("lead_time")]
        public decimal? LeadTime { get; set; }
        [NotMapped]
        public string ConditionName { get; set; }
        [NotMapped]
        public string LocationName { get; set; }

        [MaxLength(100)]
        [Column("file_attachment")]
        public string FileAttachment { get; set; } //(nvarchar(50), null)

        [NotMapped]
        public IFormFile FileUpload { get; set; }
        [NotMapped]
        public string FileAttachmentName { get; set; }

        [NotMapped]
        public string InventoryInNumber { get; set; }

        [NotMapped]
        public string InventoryName { get; set; }

        [NotMapped]
        public string UnitOfMeasure { get; set; }
        [NotMapped]
        public string Transaction { get; set; }
        [NotMapped]
        public string ReferenceNumber { get; set; }
        [NotMapped]
        public decimal QtyOut { get; set; }
        [NotMapped]
        public decimal Difference { get; set; }
    }
}
