using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models.Finance.Transaction.Inventory
{
    public class Inventory : FinanceEntityBase
    {
        public string Id { get; set; }
        public string OrganizationId { get; set; }
        public string InventoryInNumber { get; set; }
        public DateTime? InventoryInDateTime { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string InventoryGoodsReceiptId { get; set; }
        public string WarehouseId { get; set; }
        public bool? IsRefurbished { get; set; }
        public string InventoryOutId { get; set; }
        public string BusinessUnitId { get; set; }
        public string LegalEntityId { get; set; }
        public string AffiliationId { get; set; }
        public string ApprovalId { get; set; }
        public string Remark { get; set; }
        public string SKUCode { get; set; }
        public string InventoryName { get; set; }
        public string Brand { get; set; }
        public string LegalEntity { get; set; }
        public string BusinessUnit { get; set; }
        public decimal? TotalQty { get; set; }
        public decimal? QtyOnHand { get; set; }
        public decimal? QtyOutReturn { get; set; }
        public decimal? QtyBroken { get; set; }
        public decimal? QtyNoReturn { get; set; }
        public int? LeadTime { get; set; }
        public string GetFrom { get; set; }
        public string UomId { get; set; } 
        [DisplayName("UOM")]
        public string UnitName { get; set; } 
        [DisplayName("Files")]
        public string FileUploadName { get; set; }
        [DisplayName("Upload Files")]
        public string FileUpload { get; set; }

        public string Model { get; set; }
        public string Manufacture { get; set; }
        public string VendorNameId { get; set; }
        [DisplayName("Reorder Level (Unit)")]
        public string ReOrderLevel { get; set; }
        [DisplayName("Sub Brand")]
        public string SubBrand { get; set; }
        [DisplayName("Category")]
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public List<InventoryDetail> InventoryDetails { get; set; }
        public InventoryDetail InventoryDetail { get; set; }
    }
}
