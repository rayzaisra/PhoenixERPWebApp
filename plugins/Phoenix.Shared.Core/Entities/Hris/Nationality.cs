using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("nationality",Schema ="hr")]
    public partial class Nationality : GenericEntity<string>
    {
        public string nationality_name { get; set; }
    }
}