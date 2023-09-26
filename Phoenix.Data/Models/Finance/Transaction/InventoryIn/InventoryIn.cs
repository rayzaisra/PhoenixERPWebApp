using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.InventoryIn
{
    [Table("inventory_in", Schema = "fn")]
    public class InventoryInModel: FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id"), StringLength(50)]
        public string OrganizationId { get; set; } //(nvarchar(50), not null)
        [Column("inventory_in_number"), StringLength(50)]
        public string InventoryInNumber { get; set; } //(nvarchar(50), not null)
        [Column("inventory_in_datetime")]
        public DateTime InventoryInDatetime { get; set; } //(datetime2(7), not null)
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; } //(nvarchar(50), null)
        [Column("inventory_goods_receipt_id"), StringLength(50)]
        public string InventoryGoodsReceiptid { get; set; } //(nvarchar(50), null)
        [Column("warehouse_id"), StringLength(50)]
        public string WarehouseId { get; set; } //(nvarchar(50), null)
        [Column("is_refurbished")]
        public bool IsRefurbished { get; set; } //(bit, null)
        [Column("inventory_out_id")]
        public string InventoryOutId { get; set; } //(nvarchar(50), null)
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; } //(nvarchar(50), null)
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; } //(nvarchar(50), null)
        [Column("affiliation_id")]
        public string AffiliationId { get; set; } //(nvarchar(50), null)
        [Column("approval_id")]
        public string ApprovalId { get; set; } //(nvarchar(50), null)
        [Column("remark")]
        public string Remark { get; set; } //(nvarchar(450), null)
        public List<InventoryInDetailModel> Details { get; set; }
    }

    public class InventoryInJoin : InventoryInModel
    {
        public string GoodsReceiptNumber { get; set; }
        public string Warehouse { get; set; }
        public string InventoryOutNumber { get; set; }
        public string BusinessUnit { get; set; }
        public string LegalEntity { get; set; }
    }
}