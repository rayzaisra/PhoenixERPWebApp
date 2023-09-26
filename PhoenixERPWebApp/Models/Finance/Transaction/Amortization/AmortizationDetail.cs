using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class AmortizationDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Amortization Id")]
        public string AmortizationId { get; set; }

        [DisplayName("Account Name (Level 5 COA)")]
        public string CoaId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
        
        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }
               
        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        //NotMapped
        [DisplayName("Account Name (Level 5 COA)")]
        public string AccountName { get; set; }

        [DisplayName("Account ID")]
        public string CodeRec { get; set; }
        
        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Afiliation")]
        public string AfiliationName { get; set; }
    }
}