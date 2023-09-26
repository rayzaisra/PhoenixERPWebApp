using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("campaign_status",Schema ="pm")]
    public partial class CampaignStatus : GenericEntity<string>
    {
        public string campaign_status { get; set; }
        public string description { get; set; }
    }
}