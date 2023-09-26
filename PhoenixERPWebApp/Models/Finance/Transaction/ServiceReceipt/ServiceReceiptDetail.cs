using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Finance.Transaction.ServiceReceipt
{   
    public class ServiceReceiptDetail : FinanceEntityBase
    {   
        public string Id { get; set; }
        
        public string ServiceReceiptId { get; set; }
        
        public string ItemId { get; set; }
        
        public string ItemName { get; set; }
        
        public string ItemTypeId { get; set; }
        
        public string ItemTypeName { get; set; }
        
        public string CategoryId { get; set; }
        
        public string CategoryName { get; set; }
        
        public string UOMId { get; set; }
        
        public string UnitName { get; set; }
        
        public string Brannd { get; set; }
        
        public string SubBrand { get; set; }
        
        public string Description { get; set; }
        
        public string Remark { get; set; }
        
        public string FileAttachment { get; set; }
        
        public string PurchaseOrderDetailId { get; set; }
        
        public string ItemCode { get; set; }
    }
}
