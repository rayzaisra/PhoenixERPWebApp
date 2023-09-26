using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("central_resource",Schema ="dbo")]
    public partial class CentralResource : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public string name { get; set; }
        public bool is_channel { get; set; }
    }
}