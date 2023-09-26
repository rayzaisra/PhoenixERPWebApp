using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("approval",Schema ="dbo")]
    public partial class Approval : GenericEntity<string>
    {
        public string application_entity_id { get; set; }
        public string approval_group_id { get; set; }
        public string record_id { get; set; }
        public string status { get; set; }
    }
}