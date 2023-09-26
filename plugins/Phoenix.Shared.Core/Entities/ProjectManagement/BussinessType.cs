using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("bussiness_type",Schema ="pm")]
    public partial class BussinessType : GenericEntity<string>
    {
        public string propose_type { get; set; }
    }
}