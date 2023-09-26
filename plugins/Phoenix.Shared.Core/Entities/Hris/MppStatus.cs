using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("mpp_status",Schema ="hr")]
    public partial class MppStatus : GenericEntity<string>
    {
        public string name { get; set; }
    }
}