using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("cash_flow", Schema = "fn")]
    public class CashFlow : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("type"), StringLength(50)]
        public string Type { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("bank_id"), StringLength(50)]
        public string BankId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("condition"), StringLength(50)]
        public string Condition { get; set; }

        [Column("amount")]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        [Column("description"), StringLength(50)]
        public string Description { get; set; }

        [Column("cf_date")]
        public DateTime? CFDate { get; set; }

        [NotMapped]
        public string BankName { get; set; }

        [NotMapped]
        public string CurrencyName { get; set; }
    }
}