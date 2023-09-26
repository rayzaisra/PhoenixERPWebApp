using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("budget_master_header", Schema = "fn")]
    public class BudgetMasterHeader : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("budget_year")]
        public int? BudgetYear { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("budget_name"), StringLength(50)]
        public string BudgetName { get; set; }

        [Column("periode_from")]
        public DateTime? PeriodeFrom { get; set; }

        [Column("periode_to")]
        public DateTime? PeriodeTo { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column(nameof(Source)), StringLength(50)]
        public string Source { get; set; }

        [MaxLength(50)]
        [Column(nameof(Status)), StringLength(50)]
        public string Status { get; set; }

        [Column(nameof(Version))]
        public int? Version { get; set; }

        [NotMapped]
        public string CreatedByName { get; set; }

        [NotMapped]
        public string CurrencyName { get; set; }

        [NotMapped]
        public bool IsOwner { get; set; }

        [NotMapped]
        public BudgetMaster BudgetMaster { get; set; }
        [NotMapped]
        public List<BudgetMasterDetail> BudgetMasterDetail { get; set; }
    }
}