using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TeamRoleDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
        [StringLength(50)]
        [Required]
        public string team_id { get; set; }
    }
    public class TeamRoleNew
    {
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
        [StringLength(50)]
        [Required]
        public string team_id { get; set; }
    }            
    public class TeamRoleSearch : SearchParameter
    {
        public string Search_application_role_id { get; set; }
        public string Search_team_id { get; set; }
        
    }
}