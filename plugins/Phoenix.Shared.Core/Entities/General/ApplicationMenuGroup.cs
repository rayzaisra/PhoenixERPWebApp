using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("application_menu_group",Schema ="dbo")]
    public partial class ApplicationMenuGroup : GenericEntity<string>
    {
        public string group_name { get; set; }
        public string organization_id { get; set; }
    }
}