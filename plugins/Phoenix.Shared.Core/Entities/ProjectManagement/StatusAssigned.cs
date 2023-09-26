using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("status_assigned",Schema ="pm")]
    public partial class StatusAssigned : GenericEntity<string>
    {
        public string status_assigned { get; set; }
    }
}