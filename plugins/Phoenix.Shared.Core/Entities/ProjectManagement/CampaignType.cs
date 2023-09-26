using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("campaign_type",Schema ="pm")]
    public partial class CampaignType : GenericEntity<string>
    {
        public string campaign_type { get; set; }
        public string description { get; set; }
    }
}