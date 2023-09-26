using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class CampaignProgressDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string campaign_progress { get; set; }
    }
    public class CampaignProgressNew
    {
        [StringLength(50)]
        public string campaign_progress { get; set; }
    }            
    public class CampaignProgressSearch : SearchParameter
    {
        public string Search_campaign_progress { get; set; }
        
    }
}