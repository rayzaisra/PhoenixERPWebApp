using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class RequestForQuotationDetail : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Request For Quotation Id")]
        public string RequestForQuotationId { get; set; }

        [DisplayName("Vendor Id")]
        public string VendorId { get; set; }

        [DisplayName("Task Id")]
        public string TaskId { get; set; }

        [DisplayName("Parent Task Id")]
        public string ParentTaskId { get; set; }

        [DisplayName("Name")]
        public string Task { get; set; }

        [DisplayName("Type")]
        public string ItemTypeId { get; set; }

        [DisplayName("ID-Name")]
        public string ItemId { get; set; }

        [DisplayName("Code")]
        public string ItemCode { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        [DisplayName("Uom")]
        public string UomId { get; set; }

        [DisplayName("Qty")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int? Qty { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        //New
        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Operator")]
        public string Operator { get; set; }

        [DisplayName("Budget Exhcange request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalRequest { get; set; }

        [DisplayName("Original Budget request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalExchangeRequest { get; set; }

        [DisplayName("Total Price Exchange")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalExchangeAmount { get; set; }

        //Not Mapped
        [DisplayName("Uom")]
        public string UomName { get; set; }

        [DisplayName("Item Type")]
        public string ItemTypeName { get; set; }

        [DisplayName("Remaining Budget")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? RemainingBudget { get; set; }

        [DisplayName("Detail Spec")]
        public string DetailSpecFile { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencySymbolRequest { get; set; }
    }
}