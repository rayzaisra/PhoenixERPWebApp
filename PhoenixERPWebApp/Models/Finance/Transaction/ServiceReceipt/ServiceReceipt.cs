using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Finance.Transaction.ServiceReceipt
{   
    public class ServiceReceipt : FinanceEntityBase
    {   
        public string Id { get; set; }
        
        public string OrganizationId { get; set; }
        
        public string ServiceReceiptNumber { get; set; }
        
        public DateTime? ServiceReceiptDatetime { get; set; }
        
        public string JobId { get; set; }
        
        public string JobName { get; set; }
        
        public string PurchaseOrderId { get; set; }
        
        public string PurchaseOrderNumber { get; set; }
        
        public string VendorId { get; set; }
        
        public string VendorName{ get; set; }
        
        public string ReceivedBy { get; set; }

        public string ReceivedByName { get; set; }

        public DateTime? ReceivedOn { get; set; }
        
        public string DeliveryPlace { get; set; }
        
        public string DeliveryOrderNumber { get; set; }         
        
        public string DeliveryTerm{ get; set; }
        
        public string DeliveryTypeId { get; set; }
        
        public string BusinessUnitId { get; set; }
        
        public string LegalEntityId { get; set; }
        
        public string AffiliationId { get; set; }
        
        public string Remark { get; set; }
        
        public string InvoicingTypeId { get; set; }
        
        public string InvoicingTypeName { get; set; }

        //Tambahan dimasrh
        
        public string BrandId { get; set; }
        
        public string BrandName { get; set; }
        
        public string Term { get; set; }
        //[NotMapped]
        //public string JobName { get; set; }
        //[NotMapped]
        //public string PurchaseOrderNumber { get; set; }
        
        public string BusinessUnitName { get; set; }
        
        public string CompanyId { get; set; }
        
        public string CompanyName { get; set; }
        
        public string LegalEntityName { get; set; }
        
        public string ReferenceNumber { get; set; }
    }
}
