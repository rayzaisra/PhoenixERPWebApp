using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("budget_master_detail", Schema = "fn")]
    public class BudgetMasterDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("budget_master_id"), StringLength(50)]
        public string BudgetMasterId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Column("cost_sharing_allocation_percent")]
        public decimal CostSharingAllocationPercent { get; set; }

        [Column("budget_01")]
        public decimal Budget01 { get; set; }

        [Column("acr_budget_01")]
        public decimal AcrBudget01 { get; set; }

        [Column("real_budget_acr_01")]
        public decimal RealBudgetAcr01 { get; set; }

        [Column("bal_budget_01")]
        public decimal BalBudget01 { get; set; }

        [Column("used_budget_01")]
        public decimal UsedBudget01 { get; set; }

        [Column("budget_02")]
        public decimal Budget02 { get; set; }

        [Column("acr_budget_02")]
        public decimal AcrBudget02 { get; set; }

        [Column("real_budget_acr_02")]
        public decimal RealBudgetAcr02 { get; set; }

        [Column("used_budget_02")]
        public decimal UsedBudget02 { get; set; }

        [Column("bal_budget_02")]
        public decimal BalBudget02 { get; set; }

        [Column("budget_03")]
        public decimal Budget03 { get; set; }

        [Column("acr_budget_03")]
        public decimal AcrBudget03 { get; set; }

        [Column("real_budget_acr_03")]
        public decimal RealBudgetAcr03 { get; set; }

        [Column("used_budget_03")]
        public decimal UsedBudget03 { get; set; }

        [Column("bal_budget_03")]
        public decimal BalBudget03 { get; set; }

        [Column("budget_04")]
        public decimal Budget04 { get; set; }

        [Column("acr_budget_04")]
        public decimal AcrBudget04 { get; set; }

        [Column("real_budget_acr_04")]
        public decimal RealBudgetAcr04 { get; set; }

        [Column("used_budget_04")]
        public decimal UsedBudget04 { get; set; }

        [Column("bal_budget_04")]
        public decimal BalBudget04 { get; set; }

        [Column("budget_05")]
        public decimal Budget05 { get; set; }

        [Column("acr_budget_05")]
        public decimal AcrBudget05 { get; set; }

        [Column("real_budget_acr_05")]
        public decimal RealBudgetAcr05 { get; set; }

        [Column("used_budget_05")]
        public decimal UsedBudget05 { get; set; }

        [Column("bal_budget_05")]
        public decimal BalBudget05 { get; set; }

        [Column("budget_06")]
        public decimal Budget06 { get; set; }

        [Column("acr_budget_06")]
        public decimal AcrBudget06 { get; set; }

        [Column("real_budget_acr_06")]
        public decimal RealBudgetAcr06 { get; set; }

        [Column("used_budget_06")]
        public decimal UsedBudget06 { get; set; }

        [Column("bal_budget_06")]
        public decimal BalBudget06 { get; set; }

        [Column("budget_07")]
        public decimal Budget07 { get; set; }

        [Column("acr_budget_07")]
        public decimal AcrBudget07 { get; set; }

        [Column("real_budget_acr_07")]
        public decimal RealBudgetAcr07 { get; set; }

        [Column("used_budget_07")]
        public decimal UsedBudget07 { get; set; }

        [Column("bal_budget_07")]
        public decimal BalBudget07 { get; set; }

        [Column("budget_08")]
        public decimal Budget08 { get; set; }

        [Column("acr_budget_08")]
        public decimal AcrBudget08 { get; set; }

        [Column("real_budget_acr_08")]
        public decimal RealBudgetAcr08 { get; set; }

        [Column("used_budget_08")]
        public decimal UsedBudget08 { get; set; }

        [Column("bal_budget_08")]
        public decimal BalBudget08 { get; set; }

        [Column("budget_09")]
        public decimal Budget09 { get; set; }

        [Column("acr_budget_09")]
        public decimal AcrBudget09 { get; set; }

        [Column("real_budget_acr_09")]
        public decimal RealBudgetAcr09 { get; set; }

        [Column("used_budget_09")]
        public decimal UsedBudget09 { get; set; }

        [Column("bal_budget_09")]
        public decimal BalBudget09 { get; set; }

        [Column("budget_10")]
        public decimal Budget10 { get; set; }

        [Column("acr_budget_10")]
        public decimal AcrBudget10 { get; set; }

        [Column("real_budget_acr_10")]
        public decimal RealBudgetAcr10 { get; set; }

        [Column("used_budget_10")]
        public decimal UsedBudget10 { get; set; }

        [Column("bal_budget_10")]
        public decimal BalBudget10 { get; set; }

        [Column("budget_11")]
        public decimal Budget11 { get; set; }

        [Column("acr_budget_11")]
        public decimal AcrBudget11 { get; set; }

        [Column("real_budget_acr_11")]
        public decimal RealBudgetAcr11 { get; set; }

        [Column("used_budget_11")]
        public decimal UsedBudget11 { get; set; }

        [Column("bal_budget_11")]
        public decimal BalBudget11 { get; set; }

        [Column("budget_12")]
        public decimal Budget12 { get; set; }

        [Column("acr_budget_12")]
        public decimal AcrBudget12 { get; set; }

        [Column("real_budget_acr_12")]
        public decimal RealBudgetAcr12 { get; set; }

        [Column("used_budget_12")]
        public decimal UsedBudget12 { get; set; }

        [Column("bal_budget_12")]
        public decimal BalBudget12 { get; set; }

        [NotMapped]
        public string BusinessUnitName { get; set; }
    }
}