using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pca_task", Schema = "pm")]
    public class PcaTask : PmEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pca_id"), StringLength(50)]
        public string PcaId { get; set; }

        [MaxLength(255)]
        [Column("task_name"), StringLength(255)]
        public string TaskName { get; set; }

        [Column(nameof(Quantity))]
        public int? Quantity { get; set; }

        [MaxLength(50)]
        [Column("uom_id"), StringLength(50)]
        public string UomId { get; set; }

        [Column(nameof(Price))]
        public decimal? Price { get; set; }

        [Column("total_price")]
        public decimal? TotalPrice { get; set; }

        [Column("total_price_used")]
        public decimal? TotalPriceUsed { get; set; }

        [Column("total_price_balance")]
        public decimal? TotalPriceBalance { get; set; }

        [Column("is_ppn")]
        public bool? IsPpn { get; set; }

        [Column(nameof(Ppn))]
        public decimal? Ppn { get; set; }

        [Column("is_asf")]
        public bool? IsAsf { get; set; }

        [Column(nameof(Asf))]
        public decimal? Asf { get; set; }

        [Column(nameof(Pph))]
        public decimal? Pph { get; set; }
        [Column(nameof(Cost))]
        public decimal? Cost { get; set; }

        [Column(nameof(Percentage))]
        public decimal? Percentage { get; set; }

        [Column(nameof(Margin))]
        public decimal? Margin { get; set; }

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Column(nameof(Total))]
        public decimal? Total { get; set; }

        [MaxLength(50)]
        [Column(nameof(Departement)), StringLength(50)]
        public string Departement { get; set; }

        [Column("is_rate_card")]
        public bool IsRateCard { get; set; }

        [MaxLength(50)]
        [Column("parent_id"), StringLength(50)]
        public string ParentId { get; set; }

        public int Status { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        [Column("operator"), StringLength(50)]
        public string Operator { get; set; }

        [Column("total_original")]
        public decimal? TotalOriginal { get; set; }

        [NotMapped]
        public string DepartementName { get; set; }

        //[NotMapped]
        //public string UomName { get; set; }

        [NotMapped]
        public string UomSymbol { get; set; }

        [NotMapped]
        public string CurrencySymbol { get; set; }

        [NotMapped]
        public string CurrencySymbolRequest { get; set; }
    }
}