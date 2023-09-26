using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptBalanceSheet
    {
        public string account_id { get; set; }
        public string code_rec { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string name5 { get; set; }
        public decimal? balance { get; set; }
    }

    public class RptBalanceSheetComparison
    {
        public string account_id { get; set; }
        public string code_rec { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string name5 { get; set; }
        public decimal? balance_year { get; set; }
        public decimal? balance_month { get; set; }
        public decimal? balance_now { get; set; }
        public decimal? diference { get; set; }
    }

    public class RptBalanceSheetFilter
    {
        public string LegalEntityId { get; set; }
        public string BusinessUnitId { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public bool DisplayZeroBalance { get; set; }
        public string CoaLevel { get; set; }
    }

    public class RptBalanceSheetAffiliationFilter
    {
        public string AffiliationId { get; set; }
        public string Type { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public bool DisplayZeroBalance { get; set; }
        public string CoaLevel { get; set; }
    }
}
