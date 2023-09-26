using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pm_team",Schema ="pm")]
    public partial class Team : GenericEntity<string>
    {
        public string organization_id { get; set; }
        public string team_leader { get; set; }
        public string team_name { get; set; }
        public string job_id { get; set; }
        public bool? is_template { get; set; }
    }
}