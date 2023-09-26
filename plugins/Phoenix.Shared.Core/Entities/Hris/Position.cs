using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("position",Schema ="hr")]
    public partial class Position : GenericEntity<string>
    {
        public string central_resource_id { get; set; }
        public int? head { get; set; }
        public string level_position_id { get; set; }
        public string position_name { get; set; }
    }
}