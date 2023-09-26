using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Currency), Schema = "dbo")]
    public class Currency : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(400)]
        [Column("currency_name"), StringLength(400)]
        public string CurrencyName { get; set; }

        [MaxLength(5)]
        [Column("currency_code"), StringLength(5)]
        public string CurrencyCode { get; set; }

        [MaxLength(5)]
        [Column("currency_symbol"), StringLength(5)]
        public string CurrencySymbol { get; set; }

        [Column(nameof(Buy))]
        public decimal? Buy { get; set; }

        [Column(nameof(Sell))]
        public decimal? Sell { get; set; }

        [NotMapped]
        public string CurrencyIdDefault { get; set; }

        [NotMapped]
        public string CurrencySymbolDefault { get; set; }
    }

    public class CurrencyUpload
    {
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string Buy { get; set; }
        public string Sell { get; set; }
    }
}