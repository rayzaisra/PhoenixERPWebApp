using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("sub_campaign",Schema ="pm")]
    public partial class SubCampaign : GenericEntity<string>
    {
        public string client_brief_id { get; set; }
        public string job_posting_requisition_id { get; set; }
        public string subcampaign { get; set; }
    }
}