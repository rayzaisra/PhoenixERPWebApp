using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("subgroup",Schema ="hr")]
    public partial class Subgroup : GenericEntity<string>
    {
        public string group_id { get; set; }
        public string subgroup_code { get; set; }
        public string subgroup_name { get; set; }
    }
}