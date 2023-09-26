using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("role_access_menu",Schema ="dbo")]
    public partial class RoleAccessMenu : GenericEntity<string>
    {
        public string application_menu_id { get; set; }
        public string application_role_id { get; set; }
    }
}