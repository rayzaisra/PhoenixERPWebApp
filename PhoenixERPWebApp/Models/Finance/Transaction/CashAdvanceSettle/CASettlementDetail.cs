using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance
{
    public class CASettlementDetail : FinanceEntityBase
    {

        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Ca Settlement Id")]
        public string CaSettlementId { get; set; }

        [DisplayName("Ca Detail Id")]
        public string CaDetailId { get; set; }

        [DisplayName("Task Id")]
        public string TaskId { get; set; }

        [DisplayName("Task Name")]
        public string TaskName { get; set; }

        [DisplayName("Uom")]
        public string UomId { get; set; }

        [DisplayName("Qty")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int? Qty { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Budget Exhcange request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalRequest { get; set; }

        [DisplayName("Original Budget request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalExchangeRequest { get; set; }

        [DisplayName("Total Price Exchange")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalExchangeAmount { get; set; }

        [DisplayName("Date Purchasing")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        //Not Mapped
        [DisplayName("Uom")]
        public string UomName { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencySymbolRequest { get; set; }
    }
}
