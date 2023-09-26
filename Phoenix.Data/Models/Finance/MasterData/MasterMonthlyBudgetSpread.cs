using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_monthly_budget_spread", Schema = "fn")]
    public class MasterMonthlyBudgetSpread : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("master_monthly_budget_name"), StringLength(50)]
        public string MasterMonthlyBudgetName { get; set; }

        [Column("percentage_month_1")]
        public decimal? PercentageMonth1 { get; set; }

        [Column("percentage_month_2")]
        public decimal? PercentageMonth2 { get; set; }

        [Column("percentage_month_3")]
        public decimal? PercentageMonth3 { get; set; }

        [Column("percentage_month_4")]
        public decimal? PercentageMonth4 { get; set; }

        [Column("percentage_month_5")]
        public decimal? PercentageMonth5 { get; set; }

        [Column("percentage_month_6")]
        public decimal? PercentageMonth6 { get; set; }

        [Column("percentage_month_7")]
        public decimal? PercentageMonth7 { get; set; }

        [Column("percentage_month_8")]
        public decimal? PercentageMonth8 { get; set; }

        [Column("percentage_month_9")]
        public decimal? PercentageMonth9 { get; set; }

        [Column("percentage_month_10")]
        public decimal? PercentageMonth10 { get; set; }

        [Column("percentage_month_11")]
        public decimal? PercentageMonth11 { get; set; }

        [Column("percentage_month_12")]
        public decimal? PercentageMonth12 { get; set; }

        [Column("total_budget_spread")]
        public decimal? TotalBudgetSpread { get; set; }
    }
}