using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class CampaignTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string campaign_type { get; set; }
        [StringLength(250)]
        public string description { get; set; }
    }
    public class CampaignTypeNew
    {
        [StringLength(50)]
        public string campaign_type { get; set; }
        [StringLength(250)]
        public string description { get; set; }
    }            
    public class CampaignTypeSearch : SearchParameter
    {
        public string Search_campaign_type { get; set; }
        public string Search_description { get; set; }
        
    }
}