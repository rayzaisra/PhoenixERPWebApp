using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("married_status",Schema ="hr")]
    public partial class MarriedStatus : GenericEntity<string>
    {
        public string married_name { get; set; }
    }
}