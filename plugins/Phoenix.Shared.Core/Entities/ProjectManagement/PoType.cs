using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("po_type",Schema ="pm")]
    public partial class PoType : GenericEntity<string>
    {
        public string name { get; set; }
    }
}