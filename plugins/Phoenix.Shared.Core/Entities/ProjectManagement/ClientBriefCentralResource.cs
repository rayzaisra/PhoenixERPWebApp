using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_brief_central_resource",Schema ="pm")]
    public partial class ClientBriefCentralResource : GenericEntity<string>
    {
        public string central_resource_id { get; set; }
        public string job_detail_id { get; set; }
    }
}