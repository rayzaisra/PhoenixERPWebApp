using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance
{
    [Table("inventory_goods_request", Schema="fn")]
    public class GoodRequest : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]
        public string OrganizationId { get; set; }
        [Column("goods_request_number")]
        public string GoodRequestNumber { get; set; }
        [Column("goods_request_datetime")]
        public DateTime? GoodRequestDatetime { get; set; }
        [Column("completion_datetime")]
        public string CompletionDatetime { get; set; }
        [Column("job_id")]
        public string JobId { get; set; }
        [Column("requested_by")]
        public string RequestedBy { get; set; }
        [Column("requested_on")]
        public DateTime? RequestedOn { get; set; }
        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }
        [Column("legal_entity_id")]
        public string LegalEntityId { get; set; }
        [Column("affiliation_id")]
        public string AffiliationId { get; set; }
        [Column("remark")]
        public string Remark { get; set; }
        [Column("due_date")]
        public DateTime? DUeDate { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("inv_fa_id")]
        public string InvFaId { get; set; }
        [Column("inv_fa_name")]
        public string InvFaName { get; set; }
        [Column("job_name")]
        public string JobName { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }
        [Column("business_unit_name")]
        public string BusinessUnitName { get; set; }
        [Column("legal_entity_name")]
        public string LegalEntityName { get; set; }
        [Column("total_qty")]
        public decimal? TotalQty { get; set; }
        [Column("order_receipt")]
        public string OrderReceipt { get; set; }
        [Column("return_date")]
        public DateTime? ReturnDate { get; set; }
        [NotMapped]
        public string LocationTransfer { get; set; }
        [NotMapped]
        public string Purpose { get; set; }
        [Column("on_behalf_id")]
        public string OnBehalfId { get; set; }
        public string division_id { get; set; }
        public string Move_Loc { get; set; }
        public string Inv_Out_Type { get; set; }

        [NotMapped]
        public string LogDescription { get; set; }
       
        public string behalf_div { get; set; }
         
        public string behalf_jobtitle { get; set; }
        
        public string division_name { get; set; }
        public string inv_status { get; set; }
        [NotMapped]
        public string inv_status_name { get; set; }
    }

    public class ParamValueGR
    { 
        public string JobTitleName { get; set; } 
        public string DivisiName { get; set; }
        public string DivisionId { get; set; }
}
}
