using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("country",Schema ="dbo")]
    public partial class Country : GenericEntity<string>
    {
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string organization_id { get; set; }
    }
}