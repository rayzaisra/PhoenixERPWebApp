using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("user_role",Schema ="dbo")]
    public partial class UserRole : GenericEntity<string>
    {
        public string application_role_id { get; set; }
        public string application_user_id { get; set; }
    }
}