using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pm_team_member",Schema ="pm")]
    public partial class TeamMember : GenericEntity<string>
    {
        public string application_user_id { get; set; }
        public string team_id { get; set; }
        public string act_as { get; set; }
    }
}