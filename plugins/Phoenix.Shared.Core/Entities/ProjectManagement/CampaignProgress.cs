using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("campaign_progress",Schema ="pm")]
    public partial class CampaignProgress : GenericEntity<string>
    {
        public string campaign_progress { get; set; }
    }
}