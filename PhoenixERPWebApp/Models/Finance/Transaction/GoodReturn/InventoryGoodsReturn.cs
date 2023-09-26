using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.GoodReturn
{
    public class InventoryGoodsReturn
    {
        public string Id { get; set; }
        public string OrganizationId { get; set; }
        public string InventoryGoodsRequestId { get; set; }
        public string GoodsReturnNumber { get; set; }
        public string GoodRequestNumber { get;set;}
        public DateTime? GoodsReturnDateTime { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public string JobId { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? RequestedOn { get; set; }
        public string BusinessUnitId { get; set; }
        public string LegalEntityId { get; set; }
        public string AffiliationId { get; set; }
        public string Remark { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string InvFaId { get; set; }
        public string InvFaName { get; set; }
        public string InvFaNumber { get; set; }
        public string JobName { get; set; }
        public string BehalfJobTitle { get; set; }
        public string EmployeeName { get; set; }
        public string BusinessUnitName { get; set; }
        public string LegalEntityName { get; set; }
        public decimal? TotalQty { get; set; }
        public string OrderReceipt { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? GoodReceiptDatetime { get; set; }
        public string OnBehalfId { get; set; }
        public string DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string OnBehalfName { get; set; }
        public string StatusDesc { get; set; }
        public string LogDescription { get; set; }
    }
}
