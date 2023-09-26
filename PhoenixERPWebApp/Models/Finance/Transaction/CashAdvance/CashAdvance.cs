using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance
{
    public class CashAdvance : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("CA Request ID")]
        public string CaNumber { get; set; }

        [DisplayName("Date Request")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CaDate { get; set; }

        [DisplayName("Revision")]
        public int? Revision { get; set; }

        [DisplayName("Main Service Category")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Share Service")]
        public string ShareServiceId { get; set; }

        [DisplayName("Is Project Cost")]
        public bool? IsProjectCost { get; set; }

        [DisplayName("Job ID")]
        public string JobId { get; set; }

        [DisplayName("PCA No.")]
        public string PcaId { get; set; }

        [DisplayName("PCE No.")]
        public string PceId { get; set; }

        [DisplayName("Client Name")]
        public string ClientId { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Name / Company")]
        public string VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Bank Name")]
        public string PayeeBankId { get; set; }

        [DisplayName("Account Number")]
        public string PayeeAccountNumber { get; set; }

        [DisplayName("Account Name")]
        public string PayeeAccountName { get; set; }

        [DisplayName("Period From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CaPeriodFrom { get; set; }

        [DisplayName("Period To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CaPeriodTo { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Total Request")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Amount { get; set; }

        [DisplayName("Client Brand")]
        public string ClientBrandId { get; set; }

        [DisplayName("PCE Type")]
        public string PceTypeId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Operator")]
        public string Operator { get; set; }

        [DisplayName("Division")]
        public string RequestBusinessUnitId { get; set; }

        [DisplayName("Requestor")]
        public string RequestEmployeeId { get; set; }

        [DisplayName("Requestor")]
        public string RequestEmployeeName { get; set; }

        [DisplayName("Job Title")]
        public string RequestJobTitleId { get; set; }

        [DisplayName("Job Title")]
        public string RequestJobTitleName { get; set; }

        [DisplayName("Grade")]
        public string RequestJobGradeId { get; set; }

        [DisplayName("Grade")]
        public string RequestJobGradeName { get; set; }

        [DisplayName("Employee")]
        public string RequestedBy { get; set; }

        [DisplayName("Requested On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestedOn { get; set; }

        [DisplayName("On Behalf")]
        public string OnBehalfId { get; set; }

        public List<CashAdvanceDetail> CashAdvanceDetails { get; set; }

        //For Approval
        public int StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }

        //Vendor Information 
        [DisplayName("PIC")]
        public string VendorPIC { get; set; }

        [DisplayName("Address")]
        public string VendorAddress { get; set; }

        [DisplayName("Tax Registration / NPWP")]
        public string VendorTaxNumber { get; set; }


        //List Information 
        public string JobName { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategoryName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Division")]
        public string RequestBusinessUnitName { get; set; }

        [DisplayName("Settlement Number")]
        public string SettlementNumber { get; set; }


        public string PCACurrencyId { get; set; }
        public string DefaultCurrencyId { get; set; }

        public string CurrencySymbol { get; set; }
        public string CurrencySymbolExchange { get; set; }


        //for list
        [DisplayName("PO Number")]
        public string PONumber { get; set; }

        [DisplayName("Receivable Number")]
        public string InvoiceReceivableNumber { get; set; }

        [DisplayName("TAP Number")]
        public string TAPNumber { get; set; }

        [DisplayName("AP Number")]
        public string APNumber { get; set; }

        [DisplayName("CA Settlement Status")]
        public string StatusCASettlement { get; set; }

        public bool IsReverse { get; set; }
    }
}
