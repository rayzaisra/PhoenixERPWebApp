using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("master_location", Schema = "hr")]
    public partial class MasterLocation : GenericEntity<string>
    {
        public string location_name { get; set; }
        public string map_lat {get;set;}
        public string map_long { get; set; }
    }
}
