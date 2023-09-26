using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptBudget
    {
        [Key]
        public string AccountId { get; set; }
        public string CodeRec { get; set; }
        public string AccountName { get; set; }
        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string Status { get; set; }
        public int? Version { get; set; }
        public decimal Budget01 { get; set; }
        public decimal Budget02 { get; set; }
        public decimal Budget03 { get; set; }
        public decimal Budget04 { get; set; }
        public decimal Budget05 { get; set; }
        public decimal Budget06 { get; set; }
        public decimal Budget07 { get; set; }
        public decimal Budget08 { get; set; }
        public decimal Budget09 { get; set; }
        public decimal Budget10 { get; set; }
        public decimal Budget11 { get; set; }
        public decimal Budget12 { get; set; }
    }

    public class RptBudgetFilter
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
