using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("channel",Schema ="pm")]
    public partial class Channel : GenericEntity<string>
    {
        public long? central_resource_id { get; set; }
        public string channel { get; set; }
        public string note { get; set; }
        public double? value { get; set; }
    }
}