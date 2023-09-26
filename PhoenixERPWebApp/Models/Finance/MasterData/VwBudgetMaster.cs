using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class VwBudgetMaster
    {
        [DisplayName("Budget ID")]
        public string BudgetId { get; set; }
        public string MasterBudgetCodeId { get; set; }

        [DisplayName("HRIS Division")]
        public string HrisDivision { get; set; }

        [DisplayName("Budget Code")]
        public string BudgetCode { get; set; }

        [DisplayName("Budget Name")]
        public string BudgetName { get; set; }

        [DisplayName("Allocaton Ratio Master ID")]
        public string CostSharingAllocationId { get; set; }

        [DisplayName("Allocaton Ratio Master Name")]
        public string CostSharingAllocationName { get; set; }

        [DisplayName("Master Budget Monthly Spread ID")]
        public string MasterMonthlyBudgetSpreadId { get; set; }

        [DisplayName("Master Budget Monthly Spread Name")]
        public string MasterMonthlyBudgetName { get; set; }

        [DisplayName("Account ID")]
        public string AccountId { get; set; }

        [DisplayName("Account COA Level 3")]
        public string CoaLv3 { get; set; }

        [DisplayName("Account COA Level 5")]
        public string CoaLv5 { get; set; }

        [DisplayName("Budget Year")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TotalBudget { get; set; }

        [DisplayName("Budget Owner")]
        public string BudgetOwnerName { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("VERSION")]
        public int? Version { get; set; }

        [DisplayName("Account")]
        public string AccountName { get { return $"{AccountId}-{CoaLv5}"; } }
    }
}
