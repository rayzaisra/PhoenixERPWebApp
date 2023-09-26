using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptTrialBalance
    {
        public string account_id { get; set; }
        public string code_rec { get; set; }
        public string name5 { get; set; }
        public decimal? ob_debit { get; set; }
        public decimal? ob_credit { get; set; }
        public decimal? mut_debit { get; set; }
        public decimal? mut_credit { get; set; }
        public decimal? net_mutation { get; set; }
        public decimal? eb_debit { get; set; }
        public decimal? eb_credit { get; set; }
        public decimal? bal_debit { get; set; }
        public decimal? bal_credit { get; set; }
        public decimal? pnl_debit { get; set; }
        public decimal? pnl_credit { get; set; }
    }

    public class RptTrialBalanceFilter
    {
        public string LegalEntityId { get; set; }
        public string BusinessUnitId { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public bool DisplayZeroBalance { get; set; }
        public string CoaLevel { get; set; }
    }

    public class RptTrialBalanceValue
    {
        public decimal? bal_debit { get; set; }
        public decimal? bal_credit { get; set; }
        public decimal? pnl_debit { get; set; }
        public decimal? pnl_credit { get; set; }
    }
}
