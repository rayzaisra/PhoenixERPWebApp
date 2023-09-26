using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.BankPayment
{
    public class BankPayment : FinanceEntityBase
    {
        //bank payment
        public string Id { get; set; }
        
        public decimal? Amount { get; set; }
        
        public string PaymentNumber { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public string LegalEntityId { get; set; }
        
        public string BusinessUnitId { get; set; }
        
        public string AffiliationId { get; set; }
        
        public string BankPaymentMethodId { get; set; }
        
        public string ReferenceNumber { get; set; }
        
        public string BankAccountDestination { get; set; }
        
        public string PaymentRequestId { get; set; }
        
        public string CurrencyId { get; set; }
        
        public string BankPaymentTypeId { get; set; }
        
        public decimal? PaymentAmount { get; set; }
        
        public decimal? PaymentCharges { get; set; }
        
        public string VendorId { get; set; }
        
        public string AccountId { get; set; }
        
        public decimal? ExchangeRate { get; set; }
        
        public string Remarks { get; set; }
        
        public string Notes { get; set; }
        
        public string ApprovalId { get; set; }
        
        public string BankPaymentStatusId { get; set; }
        
        public string SourceDoc { get; set; }
        
        public string BankId { get; set; }
        
        public decimal? VatPercent { get; set; }
        
        public decimal? Vat { get; set; }
        
        public decimal? TaxPayable { get; set; }
        
        public decimal? TaxPayablePercent { get; set; }
        
        public decimal? AdvertisingTax { get; set; }
        
        public decimal? TotalAmount { get; set; }
        
        public string InvoiceReceivableId { get; set; }
        
        public string ReferenceNumberId { get; set; }
        
        public string JobId { get; set; }
        
        public string PurchaseRequestId { get; set; }
        
        public bool? IsMultiple { get; set; }
        public DateTime? RequestOn { get; set; }

        public decimal? BalanceAmount { get; set; }
        public string AccountIdTaxPayable { get; set; }
        public string AccountIdBankCharges { get; set; }
        public bool isApprove { get; set; }
        public string AccountIdPayTo { get; set; }




        //Invoice Received detail
        public string Inv_InvoiceReceivableNumber { get; set; }
        public string Inv_AccountPayableNumber { get; set; }
        public DateTime? Inv_InvoiceReceivableDate { get; set; }
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
        public DateTime? Ca_CaPeriodFrom { get; set; }
        public DateTime? Ca_CaPeriodTo { get; set; }
        public string Ca_CashAdvanceId { get; set; }

        //payment detail 

        public decimal? pd_Vat { get; set; }
        public decimal? pd_VatPercent { get; set; }
        public decimal? pd_NetAmount { get; set; }
        public decimal? pd_GrandTotal { get; set; }

        //untuk popup multiple AP
        public string pop_legalEntityId { get; set; }
        public string pop_ClientId { get; set; }
        public string pop_MainServiceCategoryId { get; set; }
        public string pop_BussinessUnitId { get; set; }
        public string pop_CurrencyId { get; set; }
        public string VendorName { get; set; }
    }
}
