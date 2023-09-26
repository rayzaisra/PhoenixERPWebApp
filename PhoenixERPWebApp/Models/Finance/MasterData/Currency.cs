using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Currency : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Currency Name")]
        public string CurrencyName { get; set; }

        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Currency Symbol")]
        public string CurrencySymbol { get; set; }

        [DisplayName("Buy")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Buy { get; set; }

        [DisplayName("Sell")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Sell { get; set; }

        //not mapped
        public string CurrencyIdDefault { get; set; }
        public string CurrencySymbolDefault { get; set; }
    }
}