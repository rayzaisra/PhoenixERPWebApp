using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientBriefCentralResourceDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
    }
    public class ClientBriefCentralResourceNew
    {
        [StringLength(50)]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
    }            
    public class ClientBriefCentralResourceSearch : SearchParameter
    {
        public string Search_central_resource_id { get; set; }
        public string Search_job_detail_id { get; set; }
        
    }
}