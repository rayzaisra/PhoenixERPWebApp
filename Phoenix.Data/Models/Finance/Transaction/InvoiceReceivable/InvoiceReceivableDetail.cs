using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("invoice_receivable_detail", Schema = "fn")]
    public class InvoiceReceivableDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("invoice_receivable_id"), StringLength(50)]
        public string InvoiceReceivableId { get; set; }

        [MaxLength(50)]
        [Column("task_id"), StringLength(50)]
        public string TaskId { get; set; }

        [MaxLength(255)]
        [Column("task_detail"), StringLength(255)]
        public string Task { get; set; }

        [MaxLength(50)]
        [Column("Item_type_id"), StringLength(50)]
        public string ItemTypeId { get; set; }

        [MaxLength(50)]
        [Column("item_id"), StringLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        [Column("item_code"), StringLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(150)]
        [Column(nameof(ItemName)), StringLength(150)]
        public string ItemName { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [Column(nameof(Qty))]
        public decimal? Qty { get; set; }

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Column(nameof(Amount))]
        public decimal? Amount { get; set; }

        [Column("payment_to")]
        public int? PaymentTo { get; set; }

        [Column("invoice_value")]
        public decimal? InvoiceValue { get; set; }

        [Column("amount_paid")]
        public decimal? AmountPaid { get; set; }

        [Column(nameof(Balance))]
        public decimal? Balance { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("pph"), StringLength(50)]
        public string Pph { get; set; }

        [Column("pph_amount")]
        public decimal? PphAmount { get; set; }

        [NotMapped]
        public string UomName { get; set; }
        [NotMapped]
        public string ItemTypeName { get; set; }
        [NotMapped]
        public string AccountName { get; set; }
        [NotMapped]
        public string CodeRec { get; set; }
        [NotMapped]
        public string CurrencySymbol { get; set; }
    }
}