using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("location",Schema ="hr")]
    public partial class Location : GenericEntity<string>
    {
        public string client_brief_id { get; set; }
        public int? lattitude { get; set; }
        public string location_name { get; set; }
        public int? longitude { get; set; }
    }
}