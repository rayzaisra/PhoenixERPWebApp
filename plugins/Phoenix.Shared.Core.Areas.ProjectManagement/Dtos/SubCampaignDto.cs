using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class SubCampaignDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string subcampaign { get; set; }
    }
    public class SubCampaignNew
    {
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string subcampaign { get; set; }
    }            
    public class SubCampaignSearch : SearchParameter
    {
        public string Search_client_brief_id { get; set; }
        public string Search_job_posting_requisition_id { get; set; }
        public string Search_subcampaign { get; set; }
        
    }
}