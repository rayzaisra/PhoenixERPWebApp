using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("religion",Schema ="hr")]
    public partial class Religion : GenericEntity<string>
    {
        public string religion_name { get; set; }
    }
}