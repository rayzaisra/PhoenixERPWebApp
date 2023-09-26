using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwAgingAp
    {
        public string Id { get; set; }

        [DisplayName("Date Transaction")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        [DisplayName("Date Received")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceReceivedDate { get; set; }

        public string LegalEntityId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        public string BusinessUnitId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        public string VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        public string MainserviceCategoryId { get; set; }

        [DisplayName("MSC")]
        public string MainserviceCategoryName { get; set; }

        public string JobId { get; set; }

        [DisplayName("Job ID-Name")]
        public string Jobs { get; set; }

        public string TypeOfExpenseId { get; set; }

        [DisplayName("PCE Type")]
        public string TypeOfExpenseName { get; set; }

        [DisplayName("AP Number")]
        public string APNumber { get; set; }

        public string CurrencyId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }

        [DisplayName("Reff Invoice No")]
        public string RefInvoiceNumber { get; set; }

        [DisplayName("Invoice No")]
        public string InvoiceNumber { get; set; }

        [DisplayName("Date Invoiced")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Days")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int? Days { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Current")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Current { get; set; }

        [DisplayName("01-30 days")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Value0130 { get; set; }

        [DisplayName("31-60 days")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Value3160 { get; set; }

        [DisplayName("61-90 days")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Value6190 { get; set; }

        [DisplayName("Over 90 days")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ValueMoreThan90 { get; set; }
    }

    public class VwAgingApFilter
    {
        [DisplayName("Vendor Name")]
        public string VendorId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("MSC")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("JOB-ID Name")]
        public string JobId { get; set; }

        [DisplayName("PCE Type")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("Original Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Amount")]
        public decimal? OriginalAmount { get; set; }
    }
}
