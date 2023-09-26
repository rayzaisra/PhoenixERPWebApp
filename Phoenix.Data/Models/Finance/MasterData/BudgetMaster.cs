using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("budget_master", Schema = "fn")]
    public class BudgetMaster : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("budget_master_header_id"), StringLength(50)]
        public string BudgetMasterHeaderId { get; set; }

        [MaxLength(50)]
        [Column("master_budget_code_id"), StringLength(50)]
        public string MasterBudgetCodeId { get; set; }

        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string HrisDivisionId { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("cost_sharing_allocation_id"), StringLength(50)]
        public string CostSharingAllocationId { get; set; }

        [MaxLength(50)]
        [Column("master_monthly_budget_spread_id"), StringLength(50)]
        public string MasterMonthlyBudgetSpreadId { get; set; }

        [Column("percentage_spread_month_1")]
        public decimal PercentageSpreadMonth1 { get; set; }

        [Column("percentage_spread_month_2")]
        public decimal PercentageSpreadMonth2 { get; set; }

        [Column("percentage_spread_month_3")]
        public decimal PercentageSpreadMonth3 { get; set; }

        [Column("percentage_spread_month_4")]
        public decimal PercentageSpreadMonth4 { get; set; }

        [Column("percentage_spread_month_5")]
        public decimal PercentageSpreadMonth5 { get; set; }

        [Column("percentage_spread_month_6")]
        public decimal PercentageSpreadMonth6 { get; set; }

        [Column("percentage_spread_month_7")]
        public decimal PercentageSpreadMonth7 { get; set; }

        [Column("percentage_spread_month_8")]
        public decimal PercentageSpreadMonth8 { get; set; }

        [Column("percentage_spread_month_9")]
        public decimal PercentageSpreadMonth9 { get; set; }

        [Column("percentage_spread_month_10")]
        public decimal PercentageSpreadMonth10 { get; set; }

        [Column("percentage_spread_month_11")]
        public decimal PercentageSpreadMonth11 { get; set; }

        [Column("percentage_spread_month_12")]
        public decimal PercentageSpreadMonth12 { get; set; }

        [Column("total_percentage_spread_month")]
        public decimal TotalPercentageSpreadMonth { get; set; }

        [Column("total_budget")]
        public decimal TotalBudget { get; set; }

        [Column("total_acr_budget")]
        public decimal TotalAcrBudget { get; set; }

        [Column("total_real_budget_acr")]
        public decimal TotalRealBudgetAcr { get; set; }

        [Column("total_budget_used")]
        public decimal TotalBudgetUsed { get; set; }

        [Column("total_budget_balance_acr")]
        public decimal TotalBudgetBalanceAcr { get; set; }

        [Column("total_budget_balance_real")]
        public decimal TotalBudgetBalanceReal { get; set; }

        [Column("total_budget_balance")]
        public decimal TotalBudgetBalance { get; set; }

        [Column("total_budget_used_all_transaction")]
        public decimal TotalBudgetUsedAllTransaction { get; set; }

        [Column("total_budget_balance_all_transaction")]
        public decimal TotalBudgetBalanceAllTransaction { get; set; }

        [Column("budget_owner_id")]
        public string BugetOwnerId { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [DisplayName("Version")]
        public int Version { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }

        [NotMapped]
        public string AccountCOALv3 { get; set; }

        [NotMapped]
        public string AccountCOALv5 { get; set; }

        [NotMapped]
        public string HrisDivisionName { get; set; }

        //For Approval
        [NotMapped]
        public int StatusApproval { get; set; }

        [NotMapped]
        public string RemarksProcess { get; set; }

        [NotMapped]
        public bool IsCurrentApproval { get; set; }
    }
}