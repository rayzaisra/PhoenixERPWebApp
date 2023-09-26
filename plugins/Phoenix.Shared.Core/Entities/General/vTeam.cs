using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("v_team", Schema ="pm")]
    public partial class vTeam :BaseEntity<string>
    {
        public string organization_id { get; set; }
        public string team_leader { get; set; }
        public string team_name { get; set; }
        public bool? IsJobDelete { get; set; }


    }
}