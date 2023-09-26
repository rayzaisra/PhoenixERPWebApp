using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("purchase_order_detail", Schema = "fn")]
    public class PurchaseOrderDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("purchase_order_id"), StringLength(50)]
        public string PurchaseOrderId { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [MaxLength(255)]
        [Column(nameof(Task)), StringLength(255)]
        public string Task { get; set; }

        [MaxLength(50)]
        [Column("parent_task_id"), StringLength(50)]
        public string ParentTaskId { get; set; }

        [MaxLength(50)]
        [Column("item_type_id"), StringLength(50)]
        public string ItemTypeId { get; set; }

        [MaxLength(50)]
        [Column("item_id"), StringLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        [Column("item_code"), StringLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(50)]
        [Column("item_name"), StringLength(50)]
        public string ItemName { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [Column(nameof(Qty))]
        public int? Qty { get; set; }

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [MaxLength(500)]
        [Column("description"), StringLength(500)]
        public string Description { get; set; }


        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Column("operator"), StringLength(50)]
        public string Operator { get; set; }

        [Column("total_request")]
        public decimal? TotalRequest { get; set; }

        [Column("total_exchange_request")]
        public decimal? TotalExchangeRequest { get; set; }

        [Column("total_exchange_amount")]
        public decimal? TotalExchangeAmount { get; set; }

        [NotMapped]
        public string ItemTypeName { get; set; }

        [NotMapped]
        public string UomName { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolRequest { get; set; }
    }
}