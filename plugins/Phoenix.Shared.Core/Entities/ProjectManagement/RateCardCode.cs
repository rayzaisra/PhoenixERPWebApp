using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("rate_card_code",Schema ="pm")]
    public partial class RateCardCode : GenericEntity<string>
    {
        public string code { get; set; }
        public string description { get; set; }
    }
}