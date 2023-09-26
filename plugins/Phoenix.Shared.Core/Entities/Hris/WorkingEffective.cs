using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("working_effective",Schema ="hr")]
    public partial class WorkingEffective : GenericEntity<string>
    {
        public string from_time { get; set; }
        public string to_time { get; set; }
    }
}