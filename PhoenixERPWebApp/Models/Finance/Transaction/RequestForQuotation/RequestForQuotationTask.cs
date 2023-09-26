using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class RequestForQuotationTask : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; } 

        [DisplayName("Request For Quotation Id")]
        public string RequestForQuotationId { get; set; }

        [DisplayName("Task Id")]
        public string TaskId { get; set; }

        [DisplayName("Name")]
        public string Task { get; set; }

        [DisplayName("Qty")]
        public int? Quantity { get; set; }

        [DisplayName("Uom")]
        public string UomId { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalPrice { get; set; }

        [DisplayName("Job Id")]
        public string JobId { get; set; }

        [DisplayName("Type")]
        public string ItemTypeId { get; set; }

        [DisplayName("ID-Name")]
        public string ItemId { get; set; }

        [DisplayName("Code")]
        public string ItemCode { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        //New
        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Operator")]
        public string Operator { get; set; }

        [DisplayName("Total Request")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalRequest { get; set; }

        [DisplayName("Total Exchange")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalExchange { get; set; }

        //Not Mapped
        [DisplayName("Uom")]
        public string UomName { get; set; }

        [DisplayName("Type")]
        public string ItemTypeName { get; set; }

        [DisplayName("Remaining Budget")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? RemainingBudget { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencySymbolRequest { get; set; }
    }
}