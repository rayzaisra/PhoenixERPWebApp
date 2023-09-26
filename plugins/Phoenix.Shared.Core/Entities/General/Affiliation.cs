using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("affiliation",Schema ="dbo")]
    public partial class Affiliation : GenericEntity<string>
    {
        public string affiliation_name { get; set; }
    }
}