using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RequestForQuotation : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("RFQ Number")]
        public string RequestForQuotationNumber { get; set; }

        [DisplayName("RFQ Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestForQuotationDate { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        //[DisplayName("Brand Name")]
        //public string BrandName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Request Currency")]
        public string RequestCurrencyId { get; set; }

        //[DisplayName("Exchange Rate")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        //public decimal? ExchangeRate { get; set; }

        [DisplayName("Delivery Place")]
        public string MasterLocationProcurementId { get; set; }

        [DisplayName("Delivery Place")]
        public string DeliveryPlace { get; set; }

        [DisplayName("Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }

        [DisplayName("Job ID / Purchase Request")]
        public string JobId { get; set; }

        [DisplayName("PCA ID")]
        public string JobPaId { get; set; }

        [DisplayName("PCE ID")]
        public string JobPeId { get; set; }

        [DisplayName("Prefered Vendor")]
        public bool? IsPreferedVendor { get; set; }

        [DisplayName("Purchase ID")]
        public string PurchaseRequestId { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Mainservice Category Id")]
        public string MainserviceCategoryId { get; set; }

        [DisplayName("PCE type")]
        public string TypeOfExpenseId { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Internal Or Project")]
        public string InternalOrProject { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //Not Mapped For List
        [DisplayName("Main Service Category")]
        public string MainserviceCategoryName { get; set; }

        [DisplayName("Tender")]
        public string IsPreferedVendorName { get { return IsPreferedVendor == null ? "" : IsPreferedVendor == true ? "Prefered" : "Selection"; } }

        [DisplayName("Employee")]
        public string RequestorName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Job Name")]
        public string JobName { get; set; }

        public RequestForQuotationVendor RequestForQuotationVendor { get; set; }
        public List<RequestForQuotationVendor> RequestForQuotationVendors { get; set; }
        public List<RequestForQuotationDetail> RequestForQuotationDetails { get; set; }
        public List<RequestForQuotationTask> RequestForQuotationTasks { get; set; }

        [DisplayName("Status")]
        public int StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get { if (Status == StatusTransactionName.WaitingApproval) { return "Request Approval"; } else { return Status; } } }

        //Kebutuhan buat internal PR
        [DisplayName("Budget Year")]
        public int? BudgetYear { get; set; }

        [DisplayName("Budget Owner")]
        public string BudgetOwnerId { get; set; }

        [DisplayName("Division")]
        public string BudgetDivisionId { get; set; }
        [DisplayName("Detail Spec")]
        public string DetailSpecFile { get; set; }
        public string DetailSpecFileName { get; set; }

        public decimal? GetVatPercent { get; set; }

        [DisplayName("Total")]
        public decimal? GrandTotal { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
