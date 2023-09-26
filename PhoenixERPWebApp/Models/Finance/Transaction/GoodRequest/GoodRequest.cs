using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models.Finance
{   
    public class GoodRequest : FinanceEntityBase
    {   
        public string Id { get; set; }
        
        public string OrganizationId { get; set; }
        
        public string GoodRequestNumber { get; set; }
        
        public DateTime? GoodRequestDatetime { get; set; }
        
        public string CompletionDatetime { get; set; }
        
        public string JobId { get; set; }
        
        public string RequestedBy { get; set; }
        
        public DateTime? RequestedOn { get; set; }
        
        public string BusinessUnitId { get; set; }
        
        public string LegalEntityId { get; set; }
        
        public string AffiliationId { get; set; }
        
        public string Remark { get; set; }
        
        public DateTime? DUeDate { get; set; }
        
        public int? Status { get; set; }
        
        public string Type { get; set; }
        
        public string InvFaId { get; set; }
        
        public string InvFaName { get; set; }
        
        public string JobName { get; set; }
        
        public string EmployeeName { get; set; }
        
        public string BusinessUnitName { get; set; }
        
        public string LegalEntityName { get; set; }

        public decimal? TotalQty { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string OrderReceipt { get; set; }
        [NotMapped]
        public string LocationTransfer { get; set; }
        [NotMapped]
        public string Purpose { get; set; }
        public string OnBehalfId { get; set; }
        public string division_id { get; set; } 
        public string division_name { get; set; }
        public string behalf_div { get; set; }
        public string behalf_jobtitle { get; set; }
        public string JobTitleName { get; set; }  
        public string Move_Loc { get; set; } 
        public string Inv_Out_Type { get; set; }
        public string LogDescription { get; set; }
        public string RemarkLog { get; set; }
        public string inv_status { get; set; } 
        public string DivisiName { get; set; }
        public string DivisionId { get; set; }
        public string inv_status_name { get; set; }
    }
}

