using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("travel",Schema ="hr")]
    public partial class Travel : GenericEntity<string>
    {
        public int? actual_travel_budget { get; set; }
        public int? expected_travel_budget { get; set; }
        public string purpose_of_visit { get; set; }
        public string travel_description { get; set; }
        public DateTime? travel_end_date { get; set; }
        public DateTime? travel_start_date { get; set; }
    }
}