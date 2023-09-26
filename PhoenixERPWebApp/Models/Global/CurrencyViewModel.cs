using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Global
{
    public class CurrencyViewModel
    {
        public string id { get; set; }
        public string currency_name { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public string exchange_rate { get; set; }
    }
}
