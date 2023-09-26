using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("dam_type",Schema ="pm")]
    public partial class DamType : GenericEntity<string>
    {
        public string name { get; set; }
    }
}