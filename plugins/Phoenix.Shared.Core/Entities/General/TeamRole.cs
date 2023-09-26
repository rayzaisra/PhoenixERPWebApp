using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("team_role",Schema ="dbo")]
    public partial class TeamRole : GenericEntity<string>
    {
        public string application_role_id { get; set; }
        public string team_id { get; set; }
    }
}