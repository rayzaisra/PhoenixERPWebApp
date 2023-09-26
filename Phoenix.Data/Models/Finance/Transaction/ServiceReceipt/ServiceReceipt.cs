using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.ServiceReceipt
{
    [Table("service_receipt", Schema = "fn")]
    public class ServiceReceipt : FinanceEntityBase
    {
        
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]
        public string OrganizationId { get; set; }
        [Column("service_receipt_number")]
        public string ServiceReceiptNumber { get; set; }
        [Column("service_receipt_datetime")]
        public DateTime? ServiceReceiptDatetime { get; set; }
        [Column("job_id")]
        public string JobId { get; set; }
        [Column("job_name")]
        public string JobName { get; set; }
        [Column("purchase_order_id")]
        public string PurchaseOrderId { get; set; }
        [Column("purchase_order_number")]
        public string PurchaseOrderNumber { get; set; }
        [Column("vendor_id")]
        public string VendorId { get; set; }
        [Column("vendor_name")]
        public string VendorName{ get; set; }
        [Column("received_by")]
        public string ReceivedBy { get; set; }
        [Column("received_by_name")]
        public string ReceivedByName { get; set; }
        [Column("received_on")]
        public DateTime? ReceivedOn { get; set; }
        [Column("delivery_place")]
        public string DeliveryPlace { get; set; }
        [Column("delivery_order_number")]
        public string DeliveryOrderNumber { get; set; }         
        [Column("delivery_term")]
        public string DeliveryTerm{ get; set; }
        [Column("delivery_type_id")]
        public string DeliveryTypeId { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("affiliation_id")]
        public string AffiliationId { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("invoicing_type_id")]
        public string InvoicingTypeId { get; set; }
        [Column("invoicing_type_name")]
        public string InvoicingTypeName { get; set; }

        //Tambahan dimasrh
        [Column("company_brand_id")]
        public string BrandId { get; set; }
        [NotMapped]
        public string BrandName { get; set; }
        [Column("term")]
        public string Term { get; set; }
        //[NotMapped]
        //public string JobName { get; set; }
        //[NotMapped]
        //public string PurchaseOrderNumber { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [Column("company_id")]
        public string CompanyId { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [Column("reference_number")]
        public string ReferenceNumber { get; set; }
    }
}
