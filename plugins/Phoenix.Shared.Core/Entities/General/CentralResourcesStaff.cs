using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("central_resources_staff",Schema ="dbo")]
    public partial class CentralResourcesStaff : GenericEntity<string>
    {
        public string central_resources_id { get; set; }
        public string user_id { get; set; }
    }
}