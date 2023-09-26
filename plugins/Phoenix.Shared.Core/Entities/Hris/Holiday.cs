using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("holiday",Schema ="hr")]
    public partial class Holiday : GenericEntity<string>
    {
        public DateTime? date { get; set; }
        public string name { get; set; }
    }
}