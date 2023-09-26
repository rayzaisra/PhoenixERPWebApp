using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("division",Schema ="hr")]
    public partial class Division : GenericEntity<string>
    {
        public string division_code { get; set; }
        public string division_name { get; set; }
        public string subgroupid { get; set; }
    }
}