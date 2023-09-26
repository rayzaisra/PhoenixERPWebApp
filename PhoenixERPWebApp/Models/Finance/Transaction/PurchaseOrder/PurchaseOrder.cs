using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PurchaseOrder : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("PO Number")]
        public string PurchaseOrderNumber { get; set; }

        [DisplayName("Purchase Order Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseOrderDate { get; set; }

        [DisplayName("Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Purchase ID")]
        public string PurchaseRequestId { get; set; }

        [DisplayName("Purchase Request Number")]
        public string PurchaseRequestNumber { get; set; }

        [DisplayName("Job ID")]
        public string JobId { get; set; }

        [DisplayName("Job Number")]
        public string JobNumber { get; set; }

        [DisplayName("PIC Name")]
        public string PicId { get; set; }

        [DisplayName("Email")]
        public string PicEmail { get; set; }

        [DisplayName("Vendor")]
        public string CompanyId { get; set; }

        [DisplayName("Vendor")]
        public string CompanyName { get; set; }

        [DisplayName("Company Reference")]
        public string CompanyReference { get; set; }

        [DisplayName("Vendor PIC Name")]
        public string VendorPicName { get; set; }

        [DisplayName("Number NPWP")]
        public string NumberNpwp { get; set; }

        [DisplayName("Vendor Brand")]
        public string CompanyBrandId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Discount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DiscountPercent { get; set; }

        [DisplayName("Discount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DiscountAmount { get; set; }

        //[DisplayName("Tax Amount")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? TaxAmount { get; set; }

        //[DisplayName("Tax Percent")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? TaxPercent { get; set; }

        //[DisplayName("Total Cost")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? Amount { get; set; }

        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubTotal { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatPercent { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("Grand Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? GrandTotal { get; set; }

        [DisplayName("Term 1")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PercentPaymentTerm1 { get; set; }

        [DisplayName("Term 2")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PercentPaymentTerm2 { get; set; }

        [DisplayName("Term 3")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PercentPaymentTerm3 { get; set; }

        [DisplayName("Term 1")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PaymentTerm1 { get; set; }

        [DisplayName("Term 2")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PaymentTerm2 { get; set; }

        [DisplayName("Term 3")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PaymentTerm3 { get; set; }

        [DisplayName("Invoicing")]
        public string InvoicingType { get; set; }

        [DisplayName("Payment Method")]
        public string PaymentMethodId { get; set; }

        [DisplayName("Special Request")]
        public string SpecialRequestId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Request ID")]
        public string RequestForQuotationId { get; set; }

        [DisplayName("RFQ Number")]
        public string RequestForQuotationNumber { get; set; }

        [DisplayName("Cash Advance ID")]
        public string CashAdvanceId { get; set; }

        [DisplayName("Payment Request ID")]
        public string PaymentRequestId { get; set; }

        [DisplayName("CA Settlement ID")]
        public string CashAdvanceSettlementId { get; set; }

        [DisplayName("PCE type")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //Not Mapped
        [DisplayName("Employee")]
        public string RequestorName { get; set; }

        [DisplayName("PCA ID")]
        public string JobPaId { get; set; }

        [DisplayName("PCE ID")]
        public string JobPeId { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        //Info PCA
        public string ShareServicesId { get; set; }

        //Info RFQ
        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubTotalRFQ { get; set; }

        [DisplayName("Vat Percent")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatPercentRFQ { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatRFQ { get; set; }

        [DisplayName("Grand Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? GrandTotalRFQ { get; set; }

        public List<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }

        [DisplayName("Status")]
        public int? StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }

        [DisplayName("Status")]
        public string StatusDesc
        {
            get { return PurchaseOrderStatus.StatusName(Status); }
        }

        public bool IsCancel { get; set; }

        public bool IsClose { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencySymbolExchange { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
    }
}