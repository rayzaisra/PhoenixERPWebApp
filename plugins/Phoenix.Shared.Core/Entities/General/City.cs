using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("city",Schema ="dbo")]
    public partial class City : GenericEntity<string>
    {
        public string city_code { get; set; }
        public string city_name { get; set; }
        public string country_id { get; set; }
    }
}