using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BudgetMaster : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Budget Master Header Id")]
        public string BudgetMasterHeaderId { get; set; }

        [DisplayName("Budget Code")]
        public string MasterBudgetCodeId { get; set; }

        [DisplayName("Hris Division")]
        public string HrisDivisionId { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        [DisplayName("Cost Sharing Allocation")]
        public string CostSharingAllocationId { get; set; }

        [DisplayName("Master Monthly Budget Spread")]
        public string MasterMonthlyBudgetSpreadId { get; set; }

        [DisplayName("January")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth1 { get; set; }

        [DisplayName("February")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth2 { get; set; }

        [DisplayName("March")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth3 { get; set; }

        [DisplayName("April")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth4 { get; set; }

        [DisplayName("May")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth5 { get; set; }

        [DisplayName("June")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth6 { get; set; }

        [DisplayName("July")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth7 { get; set; }

        [DisplayName("August")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth8 { get; set; }

        [DisplayName("September")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth9 { get; set; }

        [DisplayName("October")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth10 { get; set; }

        [DisplayName("November")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth11 { get; set; }

        [DisplayName("December")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageSpreadMonth12 { get; set; }

        [DisplayName("Total Spread (%)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalPercentageSpreadMonth { get; set; }

        [DisplayName("Total Budget")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalBudget { get; set; }

        [DisplayName("Total PR")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalAcrBudget { get; set; }

        [DisplayName("Total PO")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalRealBudgetAcr { get; set; }

        [DisplayName("Total AP")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalBudgetUsed { get; set; }

        [DisplayName("Total PR Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalBudgetBalanceAcr { get; set; }

        [DisplayName("Total PO Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalBudgetBalanceReal { get; set; }

        [DisplayName("Total AP Balance")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal TotalBudgetBalance { get; set; }

        [DisplayName("Total PR-PO-AP")]
        public decimal? TotalBudgetUsedAllTransaction { get; set; }

        [DisplayName("Total PR-PO-AP Balance")]
        public decimal? TotalBudgetBalanceAllTransaction { get; set; }

        [DisplayName("Owner")]
        public string BugetOwnerId { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Version")]
        public int Version { get; set; }

        [DisplayName("Account Id")]
        public string CodeRec { get; set; }

        [DisplayName("Account COA Level 3")]
        public string AccountCOALv3 { get; set; }

        [DisplayName("Account COA Level 5")]
        public string AccountCOALv5 { get; set; }

        [DisplayName("Division")]
        public string HrisDivisionName { get; set; }

        [DisplayName("Status")]
        public int StatusApproval { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }
        public bool IsCurrentApproval { get; set; }
    }
}