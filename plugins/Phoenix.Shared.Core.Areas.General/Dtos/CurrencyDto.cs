using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CurrencyDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(5)]
        public string currency_code { get; set; }
        [StringLength(400)]
        public string currency_name { get; set; }
        [StringLength(50)]
        public string currency_symbol { get; set; }
    }
    public class CurrencyNew
    {
        [StringLength(5)]
        public string currency_code { get; set; }
        [StringLength(400)]
        public string currency_name { get; set; }
        [StringLength(50)]
        public string currency_symbol { get; set; }
    }            
    public class currencySearch : SearchParameter
    {
        public string Search_currency_code { get; set; }
        public string Search_currency_name { get; set; }
 
        
    }
}