using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("propose_type",Schema ="pm")]
    public partial class ProposeType : GenericEntity<string>
    {
        public string name { get; set; }
    }
}