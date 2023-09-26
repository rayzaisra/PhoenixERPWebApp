using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.GoodReturn
{
    [Table("inventory_goods_return", Schema = "fn")]
    public class InventoryGoodsReturn: FinanceEntityBase
    {
        [Key]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]
        public string OrganizationId { get; set; }
        [Column("inventory_goods_request_id")]
        public string InventoryGoodsRequestId { get; set; }
        [Column("goods_return_number")]
        public string GoodsReturnNumber { get; set; }

        [NotMapped]
        public string GoodRequestNumber { get; set; }
        [Column("goods_return_datetime")]
        public DateTime? GoodsReturnDateTime { get; set; }
        [Column("completion_datetime")]
        public DateTime? CompletionDateTime { get; set; }
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
        public DateTime? DueDate { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("inv_fa_id")]
        public string InvFaId { get; set; }
        [Column("inv_fa_name")]
        public string InvFaName { get; set; }
        [NotMapped]
        public string InvFaNumber { get; set; }
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
        public  string OrderReceipt { get; set; }
        [Column("received_date")]
        public DateTime? ReceivedDate { get; set; }
        [Column("return_date")]
        public DateTime? ReturnDate { get; set; }
        [Column("on_behalf_id")]
        public string OnBehalfId { get; set; }
        [Column("division_id")]
        public string DivisionId { get; set; }
        [NotMapped]
        public string Purpose { get; set; }
        [NotMapped]
        public DateTime? GoodReceiptDatetime { get; set; }
        [NotMapped]
        public string BehalfJobTitle { get; set; }
        [NotMapped]
        public string DivisionName { get; set; }
        [NotMapped]
        public string OnBehalfName { get; set; }
        [NotMapped]
        public string LogDescription { get; set; }
    }
}
