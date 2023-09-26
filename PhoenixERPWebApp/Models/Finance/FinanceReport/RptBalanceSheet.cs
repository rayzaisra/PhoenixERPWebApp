using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RptBalanceSheet
    {
        public string account_id { get; set; }

        [DisplayName("Account ID")]
        public string code_rec { get; set; }

        public string name1 { get; set; }

        public string name2 { get; set; }

        [DisplayName("Account Name")]
        public string name5 { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? balance { get; set; }
    }

    public class RptBalanceSheetComparison
    {
        public string account_id { get; set; }

        [DisplayName("Account ID")]
        public string code_rec { get; set; }

        public string name1 { get; set; }

        public string name2 { get; set; }

        [DisplayName("Account Name")]
        public string name5 { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? balance_year { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? balance_month { get; set; }

        [DisplayName("Balance")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? balance_now { get; set; }

        [DisplayName("Diference")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? diference { get; set; }
    }

    public class RptBalanceSheetFilter
    {
        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndingDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        public bool DisplayZeroBalance { get; set; }

        [DisplayName("COA Level")]
        public string CoaLevel { get; set; }
    }

    public class RptBalanceSheetAffiliationFilter
    {
        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndingDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        public bool DisplayZeroBalance { get; set; }

        [DisplayName("COA Level")]
        public string CoaLevel { get; set; }
    }
}
