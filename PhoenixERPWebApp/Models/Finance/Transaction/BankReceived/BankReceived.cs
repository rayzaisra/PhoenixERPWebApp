using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class BankReceived : FinanceEntityBase
    {
        public string Id { get; set; }
        public string Vendor { get; set; }
        public string PaymentRequestNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string EVDNO { get; set; }
        public string Status { get; set; }
        public string Ref { get; set; }
        public string YourRef { get; set; }
        public string CC { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountPay { get; set; }
        public decimal? TotalAmountPay { get; set; }
        public decimal? Balance { get; set; }
        public string Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Period { get; set; }

        public string TaxStatus { get; set; }
        public string ReconStatus { get; set; }
        public bool? Recon { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentRequestDate { get; set; }

        public string PaymentTypeId { get; set; }
        public string TaxPayCoaId { get; set; }
        public string AffilitionId { get; set; }
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string CurrencyId { get; set; }
        public string TaxPayableCoa { get; set; }

        //pay to
        public string PayToVendor { get; set; }
        public string PaytoBank { get; set; }
        public string PaytoCoaId { get; set; }
        public string BankChargesCoaId { get; set; }
        public string PayToPettyCash { get; set; }
        public string PayToAccountNumber { get; set; }
        public string PayToType { get; set; }
        public string PaytoBankName { get; set; }

        public decimal? Vat { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? TaxPayable { get; set; }
        public decimal? TaxPayablePercent { get; set; }
        public decimal? AdvertisingTax { get; set; }
        public string InvoiceReceivableId { get; set; }
        public string ReferenceNumberId { get; set; }
        public string PaytoName { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Charges { get; set; }
        public string JobName { get; set; }
        public string InvoiceReceivableNumber { get; set; }
        public string Remarks { get; set; }
        public string BankName { get; set; }
        public string BankId { get; set; }
        public string Type { get; set; }
        public string PettyCashId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        public string MainServiceCategoryId { get; set; }
        public string SourceDoc { get; set; }
        public string PairingGroupId { get; set; }
        public bool? isAdvancePayment { get; set; }
        public bool? IsBalance { get; set; }


        //Invoice Received detail
        public string Inv_InvoiceReceivableNumber { get; set; }
        public string Inv_AccountPayableNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Inv_InvoiceReceivableDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Inv_DueDate { get; set; }

        public string Inv_BusinessUnitId { get; set; }
        public string Inv_LegalEntityId { get; set; }
        public string Inv_VendorId { get; set; }
        public string Inv_MainServiceCategory { get; set; }
        public string BusinessUnitName { get; set; }
        public string LegalEntityName { get; set; }
        public string Inv_CurrencyId { get; set; }
        public decimal? Inv_ExhangeRate { get; set; }
        public decimal? Inv_AllocationRatio { get; set; }
        public decimal? Inv_NetAmount { get; set; }
        public decimal? Inv_ValueAddedTaxPercent { get; set; }
        public decimal? Inv_ValueAddedTax { get; set; }
        public decimal? Inv_OtherDiscount { get; set; }
        public decimal? Inv_PromptPayment { get; set; }
        public decimal? Inv_PromptPayment2 { get; set; }
        public decimal? Inv_ValueAddedTaxOn { get; set; }
        public decimal? Inv_AdvertisingTax { get; set; }
        public decimal? Inv_TotalCostVendor { get; set; }

        //detail vendornya
        public string Inv_VendorName { get; set; }
        public string Inv_VendorNameSpan { get; set; }
        public string Inv_CompanyName { get; set; }
        public string Inv_VendorNPWP { get; set; }
        public string Inv_VendorNPWPSpan { get; set; }
        public string Inv_VendorAddress { get; set; }
        public string Inv_VendorAddressSpan { get; set; }
        public string Inv_VendorCity { get; set; }
        public string Inv_VendorCitySpan { get; set; }
        public string Inv_VendorZipCode { get; set; }
        public string Inv_VendorZipCodeSpan { get; set; }
        public string Inv_VendorContactPerson { get; set; }
        public string Inv_VendorContactPersonSpan { get; set; }
        public string Inv_VendorContactNumber { get; set; }
        public string Inv_VendorContactNumberSpan { get; set; }
        //tambahan

        //tambahan region CA
        public string Ca_JobId { get; set; }
        public string Ca_ClientName { get; set; }
        public string Ca_LegalEntityId { get; set; }
        public string Ca_JobNumber { get; set; }
        public string Ca_ProjectActivity { get; set; }
        public string Ca_VendorName { get; set; }
        public string Ca_PayeeAccountName { get; set; }
        public string Ca_PayeeAccountNumber { get; set; }
        public string Ca_PayeeBankId { get; set; }
        public string Ca_PayeeBankBranch { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Ca_CaPeriodFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Ca_CaPeriodTo { get; set; }

        public string Ca_CashAdvanceId { get; set; }
        public string Ca_pceId { get; set; }
        public string Ca_vendorId { get; set; }
        public string Ca_VendorPIC { get; set; }
        public string Ca_VendorTaxNumber { get; set; }
        public string Ca_VendorAddress { get; set; }
        public decimal? CaTotalAmount { get; set; }
        public string Ca_BusinessUnit { get; set; }
        public string Ca_BrandId { get; set; }
        public string Ca_PCETypeId { get; set; }
        public string Ca_ShareservicesId { get; set; }


        //payment detail 

        public decimal? pd_Vat { get; set; }
        public decimal? pd_VatPercent { get; set; }
        public decimal? pd_NetAmount { get; set; }
        public decimal? pd_GrandTotal { get; set; }






        public decimal? ExchangeRate { get; set; }
        public string JobId { get; set; }
        public string BusinessUnitId { get; set; }
        public string LegalEntityId { get; set; }
        public string PurchaseRequestId { get; set; }
        public bool? isApprove { get; set; }
        public string RemarkRejected { get; set; }


        //attach entitiy 
        public InvoiceReceivable invoiceReceivedList { get; set; }
        //public CashAdvance cashAdvanceList { get; set; }


        //untuk popup multiple AP
        public string pop_legalEntityId { get; set; }
        public string pop_ClientId { get; set; }
        public string pop_MainServiceCategoryId { get; set; }
        public string pop_BussinessUnitId { get; set; }
        public string pop_CurrencyId { get; set; }
        public string VendorName { get; set; }
        public bool? IsMultiple { get; set; }
        public string LogDescription { get; set; }
        public string pop_ARNumber { get; set; }


        // AR Tabel
        public string Ar_Id { get; set; }
        public string Ar_TransactionDate { get; set; }
        public string Ar_BankAccountDestination { get; set; }
        public string Ar_Amount { get; set; }
        public string Ar_LegalEntityId { get; set; }
        public string Ar_LegalEntity { get; set; }
        public string Ar_ClientId { get; set; }
        public string Ar_Status { get; set; }
        public string Ar_JobId { get; set; }
        public string Ar_CurrencyId { get; set; }
        public string Ar_JobName { get; set; }
        public string Ar_BusinessUnitId { get; set; }
        public string Ar_BusinessUnitName { get; set; }
        public string Ar_BrandId { get; set; }
        public string Ar_AccountReceivableNumber { get; set; }
        public string Ar_AffilitionId { get; set; }

        public string StatusDesc { get { return PaymentRequestStatus.GetStatus(Status); } }
        public List<AccountReceivable> ARList { get; set; }
        public List<BankReceivedDetail> Details { get; set; }
        public List<BankReceivedDetailJournal> JournalEntry { get; set; }


        public string CurrencySymbol { get; set; }
        public string CurrencyIdDefault { get; set; }
        public string CurrencySymbolDefault { get; set; }
    }

    public class BankReceivedList
    {
        public string Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentRequestDate { get; set; }
        public string PaymentRequestNumber { get; set; }
        public string ReferenceNumberId { get; set; }
        public string LegalEntityName { get; set; }
        public string VendorName { get; set; }
        public string JobName { get; set; }
        public string BusinessUnitName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrencyName { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountPay { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get { return PaymentRequestStatus.GetStatus(Status); } }
    }

    public class FilterBankReceived
    {
        [DisplayName("Status")]
        public string Status { get; set; }

        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }

        [DisplayName("Date")]
        public string DateType { get; set; }
    }
}
