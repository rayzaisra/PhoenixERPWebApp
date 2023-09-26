using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class RevaluationList : FinanceEntityBase
    {
        [DisplayName("Revaluation Number")]
        public string Id { get; set; }

        [DisplayName("Reference No")]
        public string Code { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Month & Year")]
        [DisplayFormat(DataFormatString = "{0:MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RevaluationDate { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        //For List 
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Currency Name")]
        public string CurrencyName { get; set; }

        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Symbol")]
        public string CurrencySymbol { get; set; }

        //For Approval 
        [DisplayName("Remarks")]
        public string LogDescription { get; set; } 
    }
}