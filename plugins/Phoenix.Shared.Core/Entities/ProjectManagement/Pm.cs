using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pm",Schema ="pm")]
    public partial class Pm : GenericEntity<string>
    {
        public string user_id { get; set; }
    }
}