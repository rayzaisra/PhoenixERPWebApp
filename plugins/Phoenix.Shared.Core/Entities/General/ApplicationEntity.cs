using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("application_entity",Schema ="dbo")]
    public partial class ApplicationEntity : GenericEntity<string>
    {
        public string display_name { get; set; }
        public string entity_name { get; set; }
        public string organization_id { get; set; }
    }
}