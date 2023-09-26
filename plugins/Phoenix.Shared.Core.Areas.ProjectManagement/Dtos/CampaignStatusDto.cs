using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class CampaignStatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string campaign_status { get; set; }
        [StringLength(250)]
        public string description { get; set; }
    }
    public class CampaignStatusNew
    {
        [StringLength(50)]
        public string campaign_status { get; set; }
        [StringLength(250)]
        public string description { get; set; }
    }            
    public class CampaignStatusSearch : SearchParameter
    {
        public string Search_campaign_status { get; set; }
        public string Search_description { get; set; }
        
    }
}