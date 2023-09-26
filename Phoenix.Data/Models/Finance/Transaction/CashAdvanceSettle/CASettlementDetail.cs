using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance
{
    [Table("ca_settlement_detail", Schema = "fn")]
    public class CASettlementDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ca_settlement_id"), StringLength(50)]
        public string CaSettlementId { get; set; }

        [MaxLength(50)]
        [Column("ca_detail_id"), StringLength(50)]
        public string CaDetailId { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [MaxLength(150)]
        [Column("task_name"), StringLength(150)]
        public string TaskName { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Required]
        [Column(nameof(Qty))]
        public int? Qty { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [Column("total_request")]
        public decimal? TotalRequest { get; set; }

        [Column("total_exchange_request")]
        public decimal? TotalExchangeRequest { get; set; }

        [Column("total_exchange_amount")]
        public decimal? TotalExchangeAmount { get; set; }

        [Column("purchase_date")]
        public DateTime? PurchaseDate { get; set; }

        [NotMapped]
        public string UomName { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolRequest { get; set; }        
    }
}
