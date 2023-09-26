using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    public class VwBudgetMaster
    {
        public string BudgetId { get; set; }
        public string MasterBudgetCodeId { get; set; }
        public string HrisDivision { get; set; }
        public string BudgetCode { get; set; }
        public string BudgetName { get; set; }
        public string CostSharingAllocationId { get; set; }
        public string CostSharingAllocationName { get; set; }
        public string MasterMonthlyBudgetSpreadId { get; set; }
        public string MasterMonthlyBudgetName { get; set; }
        public string AccountId { get; set; }
        public string CoaLv3 { get; set; }
        public string CoaLv5 { get; set; }
        public decimal? TotalBudget { get; set; }
        public string BudgetOwnerName { get; set; }
        public string Status { get; set; }
        public int? Version { get; set; }
    }
}
