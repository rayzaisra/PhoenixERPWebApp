using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Media
{
    [Table("plan_detail", Schema = "media")]
    public class MediaPlanDetail : MediaPlanEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Column("plan_header_id"), StringLength(50)]
        public string PlantHeaderID { get; set; }

        [Column("market_id"), StringLength(100)]
        public string MarketId { get; set; }

        [Column("media_title_id"), StringLength(50)]
        public string MediaTitleID { get; set; }

        [Column("media_type_id"), StringLength(50)]
        public string MediaTypeID { get; set; }

        [Column("calculation")]
        public decimal Calculation { get; set; }

        [Column("spec_page"), StringLength(50)]
        public string SpecPage { get; set; }

        [Column("size_column"), StringLength(50)]
        public string SizeColumn { get; set; }

        [Column("size_width")]
        public decimal SizeWidth { get; set; }

        [Column("size_fcbw")]
        public decimal SizeFcbw { get; set; }

        [Column("currency_id"), StringLength(50)]
        public string CurrencyID { get; set; }

        [Column("currency_rate")]
        public decimal CurrencyRate { get; set; }

        [Column("surcharge")]
        public decimal Surcharge { get; set; }

        [Column("gross_amount")]
        public decimal GrossAmount { get; set; }

        [Column("disc_pct")]
        public decimal DiscPct { get; set; }

        [Column("nett_amount")]
        public decimal NettAmount { get; set; }

        [Column("total_insertion")]
        public decimal TotalInsertion { get; set; }

        [Column("total_nett_amount")]
        public decimal TotalNettAmount { get; set; }

        [Column("date_plan")]
        public DateTime DatePlan { get; set; }

        [Column("start_periode")]
        public DateTime StartPeriode { get; set; }
        [Column("end_periode")]
        public DateTime EndPeriode { get; set; }
        [Column("asf_value")]
        public decimal ASFValue { get; set; }
        [Column("asf_percent")]
        public decimal ASFPercent { get; set; }
        [Column("vat_value")]
        public decimal VatValue { get; set; }

        [Column("value_bonus_amount")]
        public decimal ValueBonusAmount { get; set; }

        [Column("nett_paid")]
        public decimal NettPaid { get; set; }
        [Column("file_master_id")]
        public string FileMasterId { get; set; }
        [Column("file_name")]
        public string FileName { get; set; }
        [Column("revision")]
        public int Revision { get; set; }

        [Column("is_checked")]
        public bool? IsChecked { get; set; }

    }
}
