using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models.Finance.Transaction.Inventory
{
    public class InventoryDetail : FinanceEntityBase
    {
        [DisplayName("Invt Detail No")]
        public string Id { get; set; }

        public string InventoryId { get; set; } //(nvarchar(50), null)

        public string ItemId { get; set; } //(nvarchar(50), null)

        public string ItemName { get; set; } //(nvarchar(450), null)

        public string ItemCategoryId { get; set; } //(nvarchar(50), null)

        public string ItemCategoryName { get; set; } //(nvarchar(450), null)

        public string UomId { get; set; } //(nvarchar(50), null)

        [DisplayName("UOM")]
        public string UnitName { get; set; } //(nvarchar(450), null)

        public string Brand { get; set; } //(nvarchar(450), null)

        public string Subbrand { get; set; } //(nvarchar(450), null)

        public string Description { get; set; } //(nvarchar(450), null)

        public decimal Qty { get; set; } //(decimal(18,2), null)

        public string Remark { get; set; } //(nvarchar(450), null)

        public string GoodReceiptDetailId { get; set; }

        public DateTime? DateReceipt { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string ProductNumber { get; set; }

        public string Color { get; set; }

        public string Condition { get; set; }

        public string Status { get; set; }

        public string VendorNameId { get; set; }
        public string VendorBrandId { get; set; }

        [DisplayName("Status")]
        [NotMapped]
        public string status_name { get; set; }

        public string SkuCode { get; set; }
        public decimal? LeadTime { get; set; }

        public string Location { get; set; }
        [NotMapped]
        public string LocationName { get; set; }
        [NotMapped]
        public string ConditionName { get; set; }
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