using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("account_payable_detail", Schema = "fn")]
    public class AccountPayableDetail : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("account_payable_id"), StringLength(50)]
        public string AccountPayableId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("evident_number"), StringLength(50)]
        public string EvidentNumber { get; set; }

        [Column("payable_balance")]
        public decimal? PayableBalance { get; set; }

        [Column("amount_paid")]
        public decimal? AmountPaid { get; set; }

        [Column(nameof(Discount))]
        public decimal? Discount { get; set; }

        [Column(nameof(Tax))]
        public decimal? Tax { get; set; }

        [Column("ending_balance")]
        public decimal? EndingBalance { get; set; }

        [Column("row_index")]
        public int? RowIndex { get; set; }

        [MaxLength(150)]
        [Column(nameof(ItemName)), StringLength(150)]
        public string ItemName { get; set; }

        [MaxLength(200)]
        [Column(nameof(ItemDescription)), StringLength(200)]
        public string ItemDescription { get; set; }

        [Required]
        [Column(nameof(Qty))]
        public decimal Qty { get; set; }

        [Required]
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(nameof(Amount))]
        public decimal Amount { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [MaxLength(50)]
        [Column("Item_type_id"), StringLength(50)]
        public string ItemTypeId { get; set; }

        [MaxLength(50)]
        [Column("d_k"), StringLength(50)]
        public string DK { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("line_code"), StringLength(50)]
        public string LineCode { get; set; }

        [MaxLength(50)]
        [Column("tax_pay_coa_id"), StringLength(50)]
        public string TaxPayCoaId { get; set; }

        [Column("tax_payable_pct")]
        public decimal? TaxPayablePct { get; set; }
    }
}