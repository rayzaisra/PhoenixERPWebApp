using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class InventoryGoodsReceipt : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Goods and Service Receipt number")]
        public string GoodsReceiptNumber { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? GoodsReceiptDatetime { get; set; }

        [DisplayName("Job ID")]
        public string JobId { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Purchase Order Id")]
        public string PurchaseOrderId { get; set; }

        [DisplayName("PO Number")]
        public string PurchaseOrderNumber { get; set; }

        [DisplayName("Vendor")]
        public string VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Received By")]
        public string ReceivedBy { get; set; }

        [DisplayName("Received By Name")]
        public string ReceivedByName { get; set; }

        //[DisplayName("Received On")]
        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? ReceivedOn { get; set; }

        //[DisplayName("Delivery Place")]
        //public string DeliveryPlace { get; set; }

        //[DisplayName("Delivery Order Number")]
        //public string DeliveryOrderNumber { get; set; }

        //[DisplayName("Delivery Term")]
        //public string DeliveryTerm { get; set; }

        //[DisplayName("Delivery Type Id")]
        //public string DeliveryTypeId { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }

        [DisplayName("Affiliation Id")]
        public string AffiliationId { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Invoicing Type Id")]
        public string InvoicingTypeId { get; set; }

        [DisplayName("Invoicing Type Name")]
        public string InvoicingTypeName { get; set; }

        [DisplayName("Company Id")]
        public string CompanyId { get; set; }

        [DisplayName("Company Brand Id")]
        public string CompanyBrandId { get; set; }

        [DisplayName("Vendor Brand")]
        public string BrandName { get; set; }

        [DisplayName("Reference Number")]
        public string ReferenceNumber { get; set; }

        [DisplayName("Term")]
        public string Term { get; set; }

        [DisplayName("Location")]
        public string LocationId { get; set; }

        [DisplayName("Is Generated")]
        public bool? IsGenerated { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //[NotMapped]
        public string CompanyName { get; set; }
        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        [DisplayName("Affiliation")]
        public string AffiliationName { get; set; }
        public string PicId { get; set; }
        [DisplayName("Pic Name")]
        public string PicName { get; set; }
        public List<InventoryGoodsReceiptDetail> InventoryGoodsReceiptDetails { get; set; }

        //For List
        public string JobIdAndName { get { return $"{JobId} - {JobName}"; } }

        //for validation 
        public bool IsNonInventory { get; set; }
    }
}