using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class InvoiceReceivableDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Invoice Receivable Id")]
        public string InvoiceReceivableId { get; set; }

        [DisplayName("Task Id")]
        public string TaskId { get; set; }

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
        public decimal? Qty { get; set; }

        [DisplayName("Unit Price")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? UnitPrice { get; set; }

        [DisplayName("Total Cost")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Payment To")]
        public int? PaymentTo { get; set; }

        [DisplayName("Total Invoice")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? InvoiceValue { get; set; }

        [DisplayName("Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? AmountPaid { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        [DisplayName("Pph")]
        public string Pph { get; set; }

        [DisplayName("Pph Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PphAmount { get; set; }

        //Not Mapped
        [DisplayName("Uom")]
        public string UomName { get; set; }

        [DisplayName("Item Type")]
        public string ItemTypeName { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Account ID")]
        public string CodeRec { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
