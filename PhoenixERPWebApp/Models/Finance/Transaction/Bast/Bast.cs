using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models.Finance
{
    public class Bast : FinanceEntityBase
    {       
        public string Id { get; set; }
        
        public string OrganizationId { get; set; }
       
        public string GoodsReceiptNumber { get; set; }
       
        public DateTime? BastReceiptTime { get; set; }
       
        public string JobId { get; set; }
       
        public string PurchaseOrderId { get; set; }
       
        public string VendorName { get; set; }
       
        public string ReceivedBy { get; set; }
        
        public string Status { get; set; }
        
        public string BusinessUnitId { get; set; }
       
        public string LegalEntityId { get; set; }
        
        public string AffiliationId { get; set; }
       
        public string Remark { get; set; }
       
        public string ClientId { get; set; }
       
        public string PurchaseOrderNumber { get; set; }
        
        public string JobName { get; set; }
       
        public string VendorId { get; set; }
       
        public string ReceivedByName { get; set; }
    }
}
