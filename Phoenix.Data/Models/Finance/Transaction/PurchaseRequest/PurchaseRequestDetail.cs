using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
    [Table("purchase_request_detail", Schema = "fn")]
    public class PurchaseRequestDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("purchase_request_id"), StringLength(50)]
        public string PurchaseRequestId { get; set; }

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
        [Column(nameof(Name)), StringLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [Column("quantity")]
        public int? Qty { get; set; }

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [Column("amount_exchange")]
        public decimal? AmountExchange { get; set; }

        [NotMapped]
        public string ItemTypeName { get; set; }

        [NotMapped]
        public string UomName { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolExchange { get; set; }

        [NotMapped]
        public decimal? TotalBudgetUsed { get; set; }

        [NotMapped]
        public decimal? TotalBudgetBalance { get; set; }
    }
}
