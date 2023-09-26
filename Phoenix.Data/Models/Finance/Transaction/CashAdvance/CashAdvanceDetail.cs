using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("cash_advance_detail", Schema = "fn")]
    public class CashAdvanceDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
         
        [MaxLength(50)]
        [Column("cash_advance_id"), StringLength(50)]
        public string CashAdvanceId { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [MaxLength(450)]
        [Column("task_name"), StringLength(450)]
        public string TaskName { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [Column(nameof(Qty))]
        public int? Qty { get; set; }

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [Column("total_request")]
        public decimal? TotalRequest { get; set; }

        [NotMapped]
        public string UomName { get; set; }
               
        [NotMapped]
        public decimal? RemainingBudget { get; set; }

        [Column("total_exchange")]
        public decimal? TotalExchange { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolExchange { get; set; }
    }
}
