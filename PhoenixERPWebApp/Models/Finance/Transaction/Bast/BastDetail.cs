using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models.Finance
{
    public class BastDetail : FinanceEntityBase
    {      
        public string Id { get; set; }
       
        public string BastId { get; set; }
     
        public string GoodReceiptId { get; set; }
       
        public string ServiceReceiptId { get; set; }
        public string GoodReceiptNumber { get; set; }

        public string BusinessUnitId { get; set; }
       
        public string LegalEntityId { get; set; }
      
        public string AffiliationId { get; set; }
       
        public string Remarks { get; set; }
        
        public DateTime? DateReceipt { get; set; }
        
        public string ItemType { get; set; }
        
        public string ItemName { get; set; }
        
        public decimal? Qty { get; set; }
        
        public decimal? ReceivedQty { get; set; }
        
        public decimal? RemainingQty { get; set; }
        
        public string Location { get; set; }
        
        public string InvFaId { get; set; }

        public string BastType { get; set; }
    }
}
