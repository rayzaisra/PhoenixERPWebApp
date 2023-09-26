using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("family_status",Schema ="hr")]
    public partial class FamilyStatus : GenericEntity<string>
    {
        public string family_status_name { get; set; }
    }
}