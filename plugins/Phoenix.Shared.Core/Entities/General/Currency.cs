using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("currency",Schema ="dbo")]
    public partial class Currency : GenericEntity<string>
    {
        public string currency_code { get; set; }
        public string currency_name { get; set; }
        public string currency_symbol { get; set; }
    }
}