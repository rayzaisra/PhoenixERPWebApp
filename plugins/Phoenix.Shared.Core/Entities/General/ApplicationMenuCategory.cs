using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("application_menu_category",Schema ="dbo")]
    public partial class ApplicationMenuCategory : GenericEntity<string>
    {
        public string category_name { get; set; }
        public string organization_id { get; set; }
    }
}