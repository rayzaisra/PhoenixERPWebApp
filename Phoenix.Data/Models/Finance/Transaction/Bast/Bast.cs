using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("inventory_bast", Schema="fn")]
    public class Bast : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]
        public string OrganizationId { get; set; }
        [Column("goods_receipt_number")]
        public string GoodsReceiptNumber { get; set; }
        [Column("bast_receipt_time")]
        public DateTime? BastReceiptTime { get; set; }
        [Column("job_id")]
        public string JobId { get; set; }
        [Column("purchase_order_id")]
        public string PurchaseOrderId { get; set; }
        [Column("vendor_name")]
        public string VendorName { get; set; }
        [Column("received_by")]
        public string ReceivedBy { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("affiliation_id")]
        public string AffiliationId { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("client_id")]
        public string ClientId { get; set; }
        [Column("purchase_order_number")]
        public string PurchaseOrderNumber { get; set; }
        [Column("job_name")]
        public string JobName { get; set; }
        [Column("vendor_id")]
        public string VendorId { get; set; }
        [Column("received_by_name")]
        public string ReceivedByName { get; set; }
        [Column("rejected_by")]
        public string RejectedBy { get; set; }
        [Column("rejected_on")]
        public DateTime? RejectedOn { get; set; }
    }
}
