using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class VwAgingAr
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

        public string ClientId { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        public string MainserviceCategoryId { get; set; }

        [DisplayName("MSC")]
        public string MainserviceCategoryName { get; set; }

        public string JobId { get; set; }

        [DisplayName("Job ID-Name")]
        public string Jobs { get; set; }

        public string TypeOfExpenseId { get; set; }

        [DisplayName("PCE Type")]
        public string TypeOfExpenseName { get; set; }

        [DisplayName("Invoice No")]
        public string InvoiceNumber { get; set; }

        public string CurrencyId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        [Column("Brand Name")]
        public string BrandName { get; set; }

        [DisplayName("Delivery No")]
        public string DeliveryNote { get; set; }

        [DisplayName("Delivery Dates")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDeliveryDate { get; set; }

        [DisplayName("PO Client No")]
        public string PoClientNo { get; set; }

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

    public class VwAgingArFilter
    {
        [DisplayName("Client Name")]
        public string ClientId { get; set; }

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
