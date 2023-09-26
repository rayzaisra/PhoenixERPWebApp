using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("dam_type_status",Schema ="pm")]
    public partial class DamTypeStatus : GenericEntity<string>
    {
        public string job_status_id { get; set; }
        public string dam_type_id { get; set; }   
    }
}