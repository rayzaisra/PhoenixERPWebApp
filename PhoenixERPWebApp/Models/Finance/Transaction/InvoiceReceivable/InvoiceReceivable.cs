using Microsoft.AspNetCore.Http;
using PhoenixERPWebApp.Models.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class InvoiceReceivable : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("TAP Number")]
        public string InvoiceNumber { get; set; }

        [DisplayName("Vendor Name")]
        public string ClientId { get; set; }

        [DisplayName("Purchase Order No")]
        public string PoId { get; set; }

        [DisplayName("Reff Invoice Vendor")]
        public string InvoiceVendor { get; set; }

        [DisplayName("Reff Date Vendor")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceVendorDate { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [DisplayName("Tax Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TaxDate { get; set; }

        [DisplayName("Tax Number")]
        public string TaxNumber { get; set; }

        [DisplayName("Service and Good Receipt")]
        public string GoodReceivedId { get; set; }

        [DisplayName("Service and Good Receipt")]
        public string GoodReceivedNumber { get; set; }

        [DisplayName("Service Received")]
        public string ServiceReceivedId { get; set; }

        [DisplayName("Service Receipt Number")]
        public string ServiceReceivedNumber { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryId { get; set; }

        [DisplayName("Share Services")]
        public string ShareServicesId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Job ID / PR Request")]
        public string JobId { get; set; }

        [DisplayName("PCA ID")]
        public string PcaId { get; set; }

        [DisplayName("Allocation Ratio")]
        public string AllocationRatioId { get; set; }

        [DisplayName("Po Check List")]
        public bool? PoCheckList { get; set; }

        [DisplayName("Po Object")]
        public string PoObject { get; set; }

        [DisplayName("Tax Check List")]
        public bool? TaxCheckList { get; set; }

        [DisplayName("Tax Object")]
        public string TaxObject { get; set; }

        [DisplayName("Contract Check List")]
        public bool? ContractCheckList { get; set; }

        [DisplayName("Contract Object")]
        public string ContractObject { get; set; }

        [DisplayName("Do Check List")]
        public bool? DoCheckList { get; set; }

        [DisplayName("Do Object")]
        public string DoObject { get; set; }

        [DisplayName("Gr Check List")]
        public bool? GrCheckList { get; set; }

        [DisplayName("Gr Object")]
        public string GrObject { get; set; }

        [DisplayName("Inv Check List")]
        public bool? InvCheckList { get; set; }

        [DisplayName("Inv Object")]
        public string InvObject { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Payment")]
        public string PaymentTo { get; set; }

        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? NetAmount { get; set; }

        [DisplayName("Discount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DiscountPercent { get; set; }

        [DisplayName("Discount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? DiscountAmount { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValueAddedTaxPercent { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValueAddedTax { get; set; }

        [DisplayName("Advertising Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AdvertisingTax { get; set; }

        [DisplayName("Tax Payable Percent")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayablePercent { get; set; }

        [DisplayName("Tax Payable")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayable { get; set; }

        [DisplayName("Tax Payable Coa")]
        public string TaxPayableCoaId { get; set; }

        [DisplayName("Grand Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalCostVendor { get; set; }

        [DisplayName("Approval Front Desk")]
        public bool? ApprovalFrontDesk { get; set; }

        [DisplayName("Approval Admin Desk")]
        public bool? ApprovalAdminDesk { get; set; }

        [DisplayName("Status")]
        public string InvoiceStatusId { get; set; }

        [DisplayName("Media Order Number")]
        public string MediaOrderNumber { get; set; }

        [DisplayName("Description")]
        public string Remarks { get; set; }

        //NOT MAPPED
        //For List Invoice Received & TAP
        [DisplayName("Vendor Name")]
        public string ClientName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("AP Number")]
        public string AccountPayableNumber { get; set; }

        [DisplayName("Status")]
        public string Status { get { return InvoiceReceivedStatus.StatusName(InvoiceStatusId); } }

        //For Detail Form
        [DisplayName("Remarks")]
        public string Description { get; set; }

        [DisplayName("Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        public List<InvoiceReceivableDetail> InvoiceReceivableDetails { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public Company Company { get; set; }

        //--------------Upload File---------  
        public string filePoObjectName { get; set; }
        public string fileTaxObjectName { get; set; }
        public string fileContractObjectName { get; set; }
        public string fileDoObjectName { get; set; }
        public string fileGrObjectName { get; set; }
        public string fileInvObjectName { get; set; }

        //----------------------------------

        //Buat Nentuin balik ke list mana setelah proses crud
        public string ListGroup { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencyIdDefault { get; set; }

        public string CurrencySymbolDefault { get; set; }
    }

    public class FilterInvoiceReceivable
    {

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("TAP Number")]
        public string InvoiceReceivableId { get; set; }

        [DisplayName("AP Number")]
        public string AccountPayableNumber { get; set; }

        [DisplayName("Vendor")]
        public string VendorId { get; set; }

        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        
        [DisplayName("Date")]
        public string DateType { get; set; }
    }
}