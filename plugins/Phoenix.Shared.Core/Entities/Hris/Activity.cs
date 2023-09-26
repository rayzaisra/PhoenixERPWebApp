using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("activity",Schema ="hr")]
    public partial class Activity : GenericEntity<string>
    {
        public string code { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }
}