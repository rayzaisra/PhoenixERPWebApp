using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhoenixERPWebApp.Models.Finance
{
    public class CASettlement : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Cash Advance Id")]
        public string CashAdvanceId { get; set; }

        [DisplayName("Payment Request Id")]
        public string PaymentRequestId { get; set; }

        [DisplayName("Settlement Number")]
        public string SettlementNumber { get; set; }

        [DisplayName("Ca Settlement Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CaSettlementDate { get; set; }

        [DisplayName("SubTotal")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TotalAmount { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? CaBalance { get; set; }

        [DisplayName("From Account")]
        public string FromAccount { get; set; }

        [DisplayName("To Account")]
        public string ToAccount { get; set; }

        [DisplayName("File Receipt")]
        public string FileReceipt { get; set; }

        [DisplayName("Section Upload Evidence")]
        public string SectionUploadEvidence { get; set; }

        [DisplayName("Job ID")]
        public string JobId { get; set; }

        [DisplayName("PCA No.")]
        public string PcaId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Name / Company")]
        public string VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Pce Type")]
        public string PceTypeId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("Vatpercent")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double? VatPercent { get; set; }

        [DisplayName("Tax Payable Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayable { get; set; }

        [DisplayName("Taxpayablepercent")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TaxPayablePercent { get; set; }

        [DisplayName("Tax Payable")]
        public string TaxPayCoaId { get; set; }

        [DisplayName("Request Employee Id")]
        public string RequestEmployeeId { get; set; }

        [DisplayName("Request Employee Name")]
        public string RequestEmployeeName { get; set; }

        [DisplayName("Request Job Title Id")]
        public string RequestJobTitleId { get; set; }

        [DisplayName("Request Job Title Name")]
        public string RequestJobTitleName { get; set; }

        [DisplayName("Request Job Grade Id")]
        public string RequestJobGradeId { get; set; }

        [DisplayName("Request Job Grade Name")]
        public string RequestJobGradeName { get; set; }

        [DisplayName("Employee")]
        public string RequestedBy { get; set; }

        [DisplayName("Requested On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestedOn { get; set; }

        [DisplayName("On Behalf")]
        public string OnBehalfId { get; set; }

        [DisplayName("Bank Id")]
        public string BankId { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Ca Status Id")]
        public string Status { get; set; }

        [DisplayName("To Account Number")]
        public string ToAccountNumber { get; set; }

        [DisplayName("Transfer To")]
        public string TransferTo { get; set; }

        //For Approval 
        public int StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }


        //For Process Upload 
        public string FileReceiptName { get; set; }

        public string SectionUploadEvidenceName { get; set; }

        //For Information CA 
        public CashAdvance CashAdvance { get; set; }

        //Detail 
        public List<CASettlementDetail> CASettlementDetail { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
