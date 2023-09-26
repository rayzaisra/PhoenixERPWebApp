using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("inventory", Schema = "fn")]
    public class Inventory : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]
        public string OrganizationId { get; set; }
        [Column("inventory_in_number")]
        public string InventoryInNumber { get; set; }
        [Column("inventory_in_datetime")]
        public DateTime? InventoryInDateTime { get; set; }
        [Column("job_id")]
        public string JobId { get; set; }

        [NotMapped]
        public string JobName  { get; set; }

        [Column("inventory_goods_receipt_id")]
        public string InventoryGoodsReceiptId { get; set; }
        [Column("warehouse_id")]
        public string WarehouseId { get; set; }
        [Column("is_refurbished")]
        public bool? IsRefurbished { get; set; }
        [Column("inventory_out_id")]
        public string InventoryOutId { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("affiliation_id")]
        public string AffiliationId { get; set; }
        [Column("approval_id")]
        public string ApprovalId { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("SKU_code")]
        public string SKUCode { get; set; }
        [Column("InventoryName")]
        public string InventoryName { get; set; }
        [Column("Brand")]
        public string Brand { get; set; }
        [Column("Legal_Entity")]
        public string LegalEntity { get; set; }
        [Column("Business_Unit")]
        public string BusinessUnit { get; set; }
        [Column("total_qty")]
        public decimal? TotalQty { get; set; }
        [Column("qty_on_hand")]
        public decimal? QtyOnHand { get; set; }
        [Column("qty_out_return")]
        public decimal? QtyOutReturn { get; set; }
        [Column("qty_broken")]
        public decimal? QtyBroken { get; set; }
        [NotMapped]
        public decimal? QtyNoReturn { get; set; }
        [Column("leadtime")]
        public int? LeadTime { get; set; }
        [Column("get_from")]
        public string GetFrom { get; set; }

        [Column("uom_id")]
        public string UomId { get; set; }

        [Column("model")]
        public string Model { get; set; }

        [Column("Manufacture")]
        public string Manufacture { get; set; }

        [Column("vendor_name_id")]
        public string VendorNameId { get; set; }

        [MaxLength(100)]
        [Column("file_upload"), StringLength(100)]
        public string FileUpload { get; set; }

        [NotMapped]
        public string FileUploadName { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }

        [NotMapped]
        public List<InventoryDetail> InventoryDetail { get; set; }

        [Column("re_order_level")]
        public string ReOrderLevel { get; set; }
        [Column("sub_brand")]
        public string SubBrand { get; set; }
        [Column("category_id")]
        public string CategoryId { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public string Status { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
    }
}
