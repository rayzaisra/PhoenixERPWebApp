using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptBudgetBalance
    {
        [Key]
        public string AccountId { get; set; }
        public string CodeRec { get; set; }
        public string AccountName { get; set; }
        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal TotalPR { get; set; }
        public decimal TotalPO { get; set; }
        public decimal TotalAP { get; set; }
    }

    public class RptBudgetBalanceFilter
    {
        public int Year { get; set; }
        public string BusinessUnitId { get; set; }
        public bool DisplayAmountWaitingApproval { get; set; }
        public string Source { get; set; }
        public decimal? Version { get; set; }
        public string Status { get; set; }
        public string CoaLevel { get; set; }
    }
}
