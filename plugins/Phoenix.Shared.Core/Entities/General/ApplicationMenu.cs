using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("application_menu",Schema ="dbo")]
    public partial class ApplicationMenu : GenericEntity<string>
    {
        public string action_url { get; set; }
        public string application_menu_category_id { get; set; }
        public string application_menu_group_id { get; set; }
        public string menu_name { get; set; }
        public string organization_id { get; set; }
        public string parent_menu_id { get; set; }
    }
}