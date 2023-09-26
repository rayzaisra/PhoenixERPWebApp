using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class ViewBudgetCode
    {
        public string BudgetCode { get; set; }
        public string BudgetName { get; set; }
        public string BugetOwnerId { get; set; }
        public decimal? TotalBudgetUsed { get; set; }
        public decimal? TotalBudgetBalance { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
