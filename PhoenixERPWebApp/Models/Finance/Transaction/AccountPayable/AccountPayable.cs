using PhoenixERPWebApp.Models.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class AccountPayable : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("AP Number")]
        public string AccountPayableNumber { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affilition")]
        public string AffilitionId { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        [DisplayName("Vendor")]
        public string VendorId { get; set; }

        [DisplayName("Job ID-Name")]
        public string JobId { get; set; }

        [DisplayName("Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        [DisplayName("Currency Id")]
        public string CurrencyId { get; set; }

        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Bank Account Destination")]
        public string BankAccountDestination { get; set; }

        [DisplayName("Charges")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Charges { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("TAP Number")]
        public string InvoiceReceivableId { get; set; }

        [DisplayName("Is Journal")]
        public string IsJournal { get; set; }

        [DisplayName("D K")]
        public string DK { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Mainservice Category Id")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Company Brand Id")]
        public string CompanyBrandId { get; set; }

        [DisplayName("Tax Number")]
        public string TaxNumber { get; set; }

        [DisplayName("Periode")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Periode { get; set; }

        [DisplayName("Tax Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TaxDate { get; set; }

        [DisplayName("Invoice Vendor Number")]
        public string InvoiceVendorNumber { get; set; }

        [DisplayName("Your Ref")]
        public string YourRef { get; set; }

        [MaxLength(50)]
        [DisplayName("Allocation Ratio")]
        public string AllocationRatioId { get; set; }

        [DisplayName("Invoice Received Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceReceivedDate { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        [DisplayName("Our Ref")]
        public string OurRef { get; set; }

        [DisplayName("Our Ref Id")]
        public string OurRefId { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DueDate { get; set; }

        [DisplayName("Purchase Order Number")]
        public string PurchaseOrderId { get; set; }

        [DisplayName("Line Code")]
        public string LineCode { get; set; }

        [DisplayName("Temporary Account Payable Id")]
        public string TemporaryAccountPayableId { get; set; }

        [DisplayName("Is Payment")]
        public bool? IsPayment { get; set; }

        //NOT MAPPED
        [DisplayName("Tax Payable COA")]
        public string TaxPayableCoaId { get; set; }

        //For List AP
        [DisplayName("Vendor Name")]
        public string ClientName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get { return InvoiceReceivedStatus.StatusName(Status); } }

        //For Payment Request
        public string VendorName { get; set; }
        public decimal? Vat { get; set; }
        [DisplayName("Tax Payable")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayable { get; set; }
        public string VendorAccountNumber { get; set; }
        public string TaxPayableCoaName { get; set; }
        public string SwiftCode { get; set; }

        [DisplayName("Payment Number")]
        public string PaymentRequestNumber { get; set; }

        [DisplayName("Payment Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? PaymentRequestAmount { get; set; }

        [DisplayName("Payment Status")]
        public string PaymentRequestStatus { get; set; }

        [DisplayName("Payment Status")]
        public string PaymentRequestStatusDesc { get { return PhoenixERPWebApp.Models.PaymentRequestStatus.GetStatus(this.PaymentRequestStatus); } }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryName { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyIdDefault { get; set; }
        public string CurrencySymbolDefault { get; set; }

        //for payment request
        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountOriginal { get; set; }

        [DisplayName("Tax Payable")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayableOriginal { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatOriginal { get; set; }
    }
}