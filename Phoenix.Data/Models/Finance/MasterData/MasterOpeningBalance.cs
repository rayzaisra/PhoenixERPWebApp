using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_opening_balance", Schema = "fn")]
    public class MasterOpeningBalance : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("mob_number"), StringLength(50)]
        public string MobNumber { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("legal_entity_id"), StringLength(50)]
        public string LegalEntityId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("reff_number"), StringLength(50)]
        public string ReffNumber { get; set; }

        [MaxLength(50)]
        [Column("year_ob"), StringLength(50)]
        public string YearOb { get; set; }

        [MaxLength(50)]
        [Column("account_id"), StringLength(50)]
        public string AccountId { get; set; }

        [MaxLength(50)]
        [Column("default_currency_id"), StringLength(50)]
        public string DefaultCurrencyId { get; set; }

        [MaxLength(50)]
        [Column("currency_id"), StringLength(50)]
        public string CurrencyId { get; set; }

        [Column("original_debit")]
        public decimal? OriginalDebit { get; set; }

        [Column("original_credit")]
        public decimal? OriginalCredit { get; set; }

        [Column("exchange_rate")]
        public decimal? ExchangeRate { get; set; }

        [Required]
        [Column(nameof(Debit))]
        public decimal Debit { get; set; }

        [Required]
        [Column(nameof(Credit))]
        public decimal Credit { get; set; }

        [Column("amount_db_01")]
        public decimal? AmountDb01 { get; set; }

        [Column("amount_cr_01")]
        public decimal? AmountCr01 { get; set; }

        [Column("amount_db_02")]
        public decimal? AmountDb02 { get; set; }

        [Column("amount_cr_02")]
        public decimal? AmountCr02 { get; set; }

        [Column("amount_db_03")]
        public decimal? AmountDb03 { get; set; }

        [Column("amount_cr_03")]
        public decimal? AmountCr03 { get; set; }

        [Column("amount_db_04")]
        public decimal? AmountDb04 { get; set; }

        [Column("amount_cr_04")]
        public decimal? AmountCr04 { get; set; }

        [Column("amount_db_05")]
        public decimal? AmountDb05 { get; set; }

        [Column("amount_cr_05")]
        public decimal? AmountCr05 { get; set; }

        [Column("amount_db_06")]
        public decimal? AmountDb06 { get; set; }

        [Column("amount_cr_06")]
        public decimal? AmountCr06 { get; set; }

        [Column("amount_db_07")]
        public decimal? AmountDb07 { get; set; }

        [Column("amount_cr_07")]
        public decimal? AmountCr07 { get; set; }

        [Column("amount_db_08")]
        public decimal? AmountDb08 { get; set; }

        [Column("amount_cr_08")]
        public decimal? AmountCr08 { get; set; }

        [Column("amount_db_09")]
        public decimal? AmountDb09 { get; set; }

        [Column("amount_cr_09")]
        public decimal? AmountCr09 { get; set; }

        [Column("amount_db_10")]
        public decimal? AmountDb10 { get; set; }

        [Column("amount_cr_10")]
        public decimal? AmountCr10 { get; set; }

        [Column("amount_db_11")]
        public decimal? AmountDb11 { get; set; }

        [Column("amount_cr_11")]
        public decimal? AmountCr11 { get; set; }

        [Column("amount_db_12")]
        public decimal? AmountDb12 { get; set; }

        [Column("amount_cr_12")]
        public decimal? AmountCr12 { get; set; }

        [Column("amount_db_total")]
        public decimal? AmountDbTotal { get; set; }

        [Column("amount_cr_total")]
        public decimal? AmountCrTotal { get; set; }

        [NotMapped]
        public string AccountName { get; set; }

        [NotMapped]
        public string CurrencyName { get; set; }

        [NotMapped]
        public string CodeRec { get; set; }
    }

    public class MasterOpeningBalanceFilter
    {
        public string FilterBusinessUnitId { get; set; }

        public string FilterLegalEntityId { get; set; }

        public string FilterYear { get; set; }

        public string FilterCoaId { get; set; }

        public string FilterCurrencyId { get; set; }
    }
}