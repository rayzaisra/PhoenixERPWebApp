using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("budget", Schema = "fn")]
    public class Budget : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("budget_number"), StringLength(50)]
        public string BudgetNumber { get; set; }

        [Required]
        [Column("financial_year_id")]
        public string FinancialYearId { get; set; }

        [NotMapped]
        public int? FinancialYearName { get; set; }

        [NotMapped]
        public string employee_basic_info_id { get; set; }

        [Required]
        [Column("organization_id")]
        public string organization_id { get; set; }

        [MaxLength(50)]
        [Column("financial_month")]
        public string FinancialMonth { get; set; }

        [MaxLength(50)]
        [Column("code")]
        public string code { get; set; }

        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string account_id { get; set; }

        [Column("currency_id")]
        public string currency_id { get; set; }

        [MaxLength(50)]
        [Column("exchange_rate"), StringLength(50)]
        public int? exchange_rate { get; set; }

        [MaxLength(50)]
        [Column("budget_value"), StringLength(50)]
        public double? budget_value { get; set; }

        [MaxLength(50)]
        [Column("budget_usage"), StringLength(50)]
        public double? budget_usage { get; set; }

        [MaxLength(50)]
        [Column("revision"), StringLength(50)]
        public int? revision { get; set; }

        [Column("budget_01")]
        public decimal? budget_01 { get; set; }

        [Column("budget_02")]
        public decimal? budget_02 { get; set; }

        [Column("budget_03")]
        public decimal? budget_03 { get; set; }

        [Column("budget_04")]
        public decimal? budget_04 { get; set; }

        [Column("budget_05")]
        public decimal? budget_05 { get; set; }

        [Column("budget_06")]
        public decimal? budget_06 { get; set; }

        [Column("budget_07")]
        public decimal? budget_07 { get; set; }

        [Column("budget_08")]
        public decimal? budget_08 { get; set; }

        [Column("budget_09")]
        public decimal? budget_09 { get; set; }

        [Column("budget_10")]
        public decimal? budget_10 { get; set; }

        [Column("budget_11")]
        public decimal? budget_11 { get; set; }

        [Column("budget_12")]
        public decimal? budget_12 { get; set; }

        [NotMapped]
        public decimal? TotalBudget { get; set; }

        [NotMapped]
        public decimal? Actual { get; set; }

        [NotMapped]
        public decimal? Balance { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }

        [NotMapped]
        public string AccountName { get; set; }
    }
}
