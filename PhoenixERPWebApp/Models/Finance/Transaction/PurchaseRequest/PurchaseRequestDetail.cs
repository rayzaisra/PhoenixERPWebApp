using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class PurchaseRequestDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Purchase Request Id")]
        public string PurchaseRequestId { get; set; }

        [DisplayName("Type")]
        public string ItemTypeId { get; set; }

        [DisplayName("ID-Name")]
        public string ItemId { get; set; }

        [DisplayName("Code")]
        public string ItemCode { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Uom")]
        public string UomId { get; set; }

        [DisplayName("Qty")]
        public int? Qty { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Total Price Exchange")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? AmountExchange { get; set; }

        //Not Mapped
        [DisplayName("Uom")]
        public string UomName { get; set; }

        [DisplayName("Item Type")]
        public string ItemTypeName { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencySymbolExchange { get; set; }

        [DisplayName("Total used")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalBudgetUsed { get; set; }

        [DisplayName("Remaining Budget")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalBudgetBalance { get; set; }
    }
}
