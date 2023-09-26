using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class PurchaseRequest : FinanceEntityBase
    {

        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("PR Number")]
        public string RequestNumber { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDatetime { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Operator")]
        public string Operator { get; set; }

        [DisplayName("Sub Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? SubTotal { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? VatPercent { get; set; }

        [DisplayName("Vat")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Vat { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? GrandTotal { get; set; }

        [DisplayName("Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Budget Year")]
        public int? BudgetYear { get; set; }

        [DisplayName("Budget Owner")]
        public string BudgetOwnerId { get; set; }

        [DisplayName("Division")]
        public string RequestorDivisionId { get; set; }

        [DisplayName("Document Specification")]
        public string DocumentSpecificationFile { get; set; }

        //Not Mapped
        [DisplayName("Employee")]
        public string Requestor { get; set; }

        [DisplayName("Division")]
        public string RequestorDivisionName { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
        public List<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }

        //For Approval 
        public int StatusApproval { get; set; }

        [DisplayName("Remarks")]
        [MaxLength(255)]
        public string RemarksProcess { get; set; }

        public string DocumentSpecificationFileName { get; set; }
        public string CurrencySymbol { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }        
    }
}
