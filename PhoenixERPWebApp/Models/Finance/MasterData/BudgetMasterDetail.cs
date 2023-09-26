using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BudgetMasterDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Budget Master Id")]
        public string BudgetMasterId { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Percent(%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CostSharingAllocationPercent { get; set; }

        [DisplayName("Budget 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget01 { get; set; }

        [DisplayName("Acr Budget 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget01 { get; set; }

        [DisplayName("Real Budget Acr 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr01 { get; set; }

        [DisplayName("Bal Budget 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget01 { get; set; }

        [DisplayName("Used Budget 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget01 { get; set; }

        [DisplayName("Budget 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget02 { get; set; }

        [DisplayName("Acr Budget 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget02 { get; set; }

        [DisplayName("Real Budget Acr 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr02 { get; set; }

        [DisplayName("Used Budget 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget02 { get; set; }

        [DisplayName("Bal Budget 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget02 { get; set; }

        [DisplayName("Budget 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget03 { get; set; }

        [DisplayName("Acr Budget 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget03 { get; set; }

        [DisplayName("Real Budget Acr 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr03 { get; set; }

        [DisplayName("Used Budget 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget03 { get; set; }

        [DisplayName("Bal Budget 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget03 { get; set; }

        [DisplayName("Budget 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget04 { get; set; }

        [DisplayName("Acr Budget 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget04 { get; set; }

        [DisplayName("Real Budget Acr 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr04 { get; set; }

        [DisplayName("Used Budget 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget04 { get; set; }

        [DisplayName("Bal Budget 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget04 { get; set; }

        [DisplayName("Budget 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget05 { get; set; }

        [DisplayName("Acr Budget 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget05 { get; set; }

        [DisplayName("Real Budget Acr 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr05 { get; set; }

        [DisplayName("Used Budget 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget05 { get; set; }

        [DisplayName("Bal Budget 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget05 { get; set; }

        [DisplayName("Budget 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget06 { get; set; }

        [DisplayName("Acr Budget 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget06 { get; set; }

        [DisplayName("Real Budget Acr 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr06 { get; set; }

        [DisplayName("Used Budget 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget06 { get; set; }

        [DisplayName("Bal Budget 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget06 { get; set; }

        [DisplayName("Budget 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget07 { get; set; }

        [DisplayName("Acr Budget 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget07 { get; set; }

        [DisplayName("Real Budget Acr 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr07 { get; set; }

        [DisplayName("Used Budget 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget07 { get; set; }

        [DisplayName("Bal Budget 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget07 { get; set; }

        [DisplayName("Budget 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget08 { get; set; }

        [DisplayName("Acr Budget 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget08 { get; set; }

        [DisplayName("Real Budget Acr 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr08 { get; set; }

        [DisplayName("Used Budget 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget08 { get; set; }

        [DisplayName("Bal Budget 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget08 { get; set; }

        [DisplayName("Budget 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget09 { get; set; }

        [DisplayName("Acr Budget 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget09 { get; set; }

        [DisplayName("Real Budget Acr 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr09 { get; set; }

        [DisplayName("Used Budget 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget09 { get; set; }

        [DisplayName("Bal Budget 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget09 { get; set; }

        [DisplayName("Budget 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget10 { get; set; }

        [DisplayName("Acr Budget 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget10 { get; set; }

        [DisplayName("Real Budget Acr 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr10 { get; set; }

        [DisplayName("Used Budget 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget10 { get; set; }

        [DisplayName("Bal Budget 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget10 { get; set; }

        [DisplayName("Budget 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget11 { get; set; }

        [DisplayName("Acr Budget 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget11 { get; set; }

        [DisplayName("Real Budget Acr 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr11 { get; set; }

        [DisplayName("Used Budget 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget11 { get; set; }

        [DisplayName("Bal Budget 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget11 { get; set; }

        [DisplayName("Budget 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget12 { get; set; }

        [DisplayName("Acr Budget 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal AcrBudget12 { get; set; }

        [DisplayName("Real Budget Acr 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal RealBudgetAcr12 { get; set; }

        [DisplayName("Used Budget 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal UsedBudget12 { get; set; }

        [DisplayName("Bal Budget 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal BalBudget12 { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }
    }
}