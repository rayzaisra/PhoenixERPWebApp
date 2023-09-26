using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterOpeningBalance : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Number")]
        public string MobNumber { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }

        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Reff Number")]
        public string ReffNumber { get; set; }

        [DisplayName("Year Ob")]
        public string YearOb { get; set; }

        [DisplayName("Account Id")]
        public string AccountId { get; set; }

        //[MaxLength(50)]
        //[Column("default_currency_id"), StringLength(50)]
        //public string DefaultCurrencyId { get; set; }

        [DisplayName("Default Currency")]
        public string DefaultCurrencyId { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }

        [DisplayName("Original Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? OriginalDebit { get; set; }

        [DisplayName("Original Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? OriginalCredit { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Debit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Debit { get; set; }

        [DisplayName("Credit")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Credit { get; set; }

        [DisplayName("Amount Db 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb01 { get; set; }

        [DisplayName("Amount Cr 01")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr01 { get; set; }

        [DisplayName("Amount Db 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb02 { get; set; }

        [DisplayName("Amount Cr 02")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr02 { get; set; }

        [DisplayName("Amount Db 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb03 { get; set; }

        [DisplayName("Amount Cr 03")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr03 { get; set; }

        [DisplayName("Amount Db 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb04 { get; set; }

        [DisplayName("Amount Cr 04")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr04 { get; set; }

        [DisplayName("Amount Db 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb05 { get; set; }

        [DisplayName("Amount Cr 05")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr05 { get; set; }

        [DisplayName("Amount Db 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb06 { get; set; }

        [DisplayName("Amount Cr 06")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr06 { get; set; }

        [DisplayName("Amount Db 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb07 { get; set; }

        [DisplayName("Amount Cr 07")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr07 { get; set; }

        [DisplayName("Amount Db 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb08 { get; set; }

        [DisplayName("Amount Cr 08")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr08 { get; set; }

        [DisplayName("Amount Db 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb09 { get; set; }

        [DisplayName("Amount Cr 09")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr09 { get; set; }

        [DisplayName("Amount Db 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb10 { get; set; }

        [DisplayName("Amount Cr 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr10 { get; set; }

        [DisplayName("Amount Db 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb11 { get; set; }

        [DisplayName("Amount Cr 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr11 { get; set; }

        [DisplayName("Amount Db 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDb12 { get; set; }

        [DisplayName("Amount Cr 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCr12 { get; set; }

        [DisplayName("Amount Db Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountDbTotal { get; set; }

        [DisplayName("Amount Cr Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountCrTotal { get; set; }

        [DisplayName("AccountID-Name")]
        public string AccountName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        [DisplayName("Code")]
        public string CodeRec { get; set; }
    }

    public class MasterOpeningBalanceFilter
    {
        [DisplayName("Business Unit")]
        public string FilterBusinessUnitId { get; set; }

        [DisplayName("Legal Entity")]
        public string FilterLegalEntityId { get; set; }

        [DisplayName("Year")]
        public string FilterYear { get; set; }

        [DisplayName("Search Code")]
        public string FilterCoaId { get; set; }

        [DisplayName("Currency Default")]
        public string FilterCurrencyId { get; set; }
    }
}