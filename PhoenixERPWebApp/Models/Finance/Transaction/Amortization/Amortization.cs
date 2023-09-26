using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Amortization : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Reference No")]
        public string Code { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AmotizationDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        //[DisplayName("Amotization Reference No")]
        //public string AmotizationReferenceNo { get; set; }

        [DisplayName("Description")]
        public string AmotizationDescription { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
        
        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Status")]
        public bool? IsDraft { get; set; }

        [DisplayName("Account Name")]
        public string CoaId { get; set; }

        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Amount")]
        public decimal? Amount { get; set; }

        [DisplayName("Allocation Ratio")]
        public string CostSharingAllocationId { get; set; }

        //Not Mapped
        public List<AmortizationDetail> AmortizationDetails { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        public DateTime? AmotizationDateEnd { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Afiliation")]
        public string AfiliationName { get; set; }
    }
}