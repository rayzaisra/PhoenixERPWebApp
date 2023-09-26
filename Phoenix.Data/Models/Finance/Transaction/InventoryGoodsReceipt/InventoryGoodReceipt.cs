using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("inventory_goods_receipt", Schema = "fn")]
    public class InventoryGoodsReceipt : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
         
        [MaxLength(50)]
        [Column("goods_receipt_number"), StringLength(50)]
        public string GoodsReceiptNumber { get; set; }
         
        [Column("goods_receipt_datetime")]
        public DateTime GoodsReceiptDatetime { get; set; }

        [MaxLength(50)]
        [Column("job_id"), StringLength(50)]
        public string JobId { get; set; }

        [MaxLength(100)]
        [Column("job_name"), StringLength(100)]
        public string JobName { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_id"), StringLength(50)]
        public string PurchaseOrderId { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_number"), StringLength(50)]
        public string PurchaseOrderNumber { get; set; }

        [MaxLength(50)]
        [Column("vendor_id"), StringLength(50)]
        public string VendorId { get; set; }

        [MaxLength(450)]
        [Column("vendor_name"), StringLength(450)]
        public string VendorName { get; set; }

        [MaxLength(50)]
        [Column("received_by"), StringLength(50)]
        public string ReceivedBy { get; set; }

        [MaxLength(50)]
        [Column("received_by_name"), StringLength(50)]
        public string ReceivedByName { get; set; }

        //[Column("received_on")]
        //public DateTime? ReceivedOn { get; set; }

        //[MaxLength(450)]
        //[Column("delivery_place"), StringLength(450)]
        //public string DeliveryPlace { get; set; }

        //[MaxLength(50)]
        //[Column("delivery_order_number"), StringLength(50)]
        //public string DeliveryOrderNumber { get; set; }

        //[MaxLength(450)]
        //[Column("delivery_term"), StringLength(450)]
        //public string DeliveryTerm { get; set; }

        //[MaxLength(50)]
        //[Column("delivery_type_id"), StringLength(50)]
        //public string DeliveryTypeId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(450)]
        [Column(nameof(Remark)), StringLength(450)]
        public string Remark { get; set; }

        [MaxLength(50)]
        [Column("invoicing_type_id"), StringLength(50)]
        public string InvoicingTypeId { get; set; }

        [MaxLength(100)]
        [Column("invoicing_type_name"), StringLength(100)]
        public string InvoicingTypeName { get; set; }

        [MaxLength(50)]
        [Column("company_id"), StringLength(50)]
        public string CompanyId { get; set; }

        [MaxLength(50)]
        [Column("company_brand_id"), StringLength(50)]
        public string CompanyBrandId { get; set; }

        [MaxLength(100)]
        [Column("brand_name"), StringLength(100)]
        public string BrandName { get; set; }

        [MaxLength(100)]
        [Column("reference_number"), StringLength(100)]
        public string ReferenceNumber { get; set; }

        [MaxLength(100)]
        [Column(nameof(Term)), StringLength(100)]
        public string Term { get; set; }

        [MaxLength(50)]
        [Column("location_id"), StringLength(50)]
        public string LocationId { get; set; }

        [Column("is_generated")]
        public bool? IsGenerated { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string AffiliationName { get; set; }
        [NotMapped]
        public string PicId { get; set; }
        [NotMapped]
        public string PicName { get; set; }

        [NotMapped]
        public List<InventoryGoodsReceiptDetail> InventoryGoodsReceiptDetails { get; set; }

        //for validation 
        [NotMapped]
        public bool IsNonInventory { get; set; }
    }
}