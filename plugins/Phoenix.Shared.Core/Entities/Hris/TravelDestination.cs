using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("travel_destination",Schema ="hr")]
    public partial class TravelDestination : GenericEntity<string>
    {
        public string arrangement_type { get; set; }
        public string place_of_visit { get; set; }
        public string travel_id { get; set; }
        public string travel_mode { get; set; }
    }
}