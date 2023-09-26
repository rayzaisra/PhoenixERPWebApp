using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("rate_card",Schema ="pm")]
    public partial class RateCard : GenericEntity<string>
    {
        public string description { get; set; }
        public string code { get; set; }
        public string business_unit_id { get; set; }
        public string detail { get; set; }
        public string name { get; set; }
        public string revision { get; set; }
        public double? total { get; set; }
        public string currency { get; set; }
    }
}