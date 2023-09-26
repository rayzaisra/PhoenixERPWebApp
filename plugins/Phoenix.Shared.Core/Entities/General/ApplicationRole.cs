using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("application_role",Schema ="dbo")]
    public partial class ApplicationRole : GenericEntity<string>
    {
        public bool is_default_role { get; set; }
        public string organization_id { get; set; }
        public string role_name { get; set; }
    }
}