using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Finance
{   
    public class GoodRequestDetail : FinanceEntityBase
    {   
        public string Id { get; set; }
        
        public string GoodRequestId { get; set; }
        
        public string ItemId { get; set; }
        
        public string ItemName { get; set; }
        
        public string TaskId { get; set; }
        
        public string TaskDetail { get; set; }
        
        public string ItemCategoryId { get; set; }
        
        public string ItemCategoryName { get; set; }
        
        public string UomId { get; set; }
        
        public string UomName { get; set; }
        
        public string Brand { get; set; }
        
        public string Subbrand { get; set; }
        
        public string Description { get; set; }
        
        public decimal? qty { get; set; }
        
        public string Remark { get; set; }
        
        public string FileAttachment { get; set; }
        
        public string Status { get; set; }
        
        public string WarehouseIdFrom { get; set; }
        
        public string WarehouseIdTo { get; set; }

        public string Purpose { get; set; }
        
        public string InvFaDetailId { get; set; }

        public string Location { get; set; }

        public string LocationName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)] 
        public DateTime? ReturnDate { get; set; }
        public string Return_detail { get; set; }
        public decimal qtyMax { get; set; }

    }
    
}
