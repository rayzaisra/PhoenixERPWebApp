using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobStatusSetDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string job_status_set { get; set; }
        [StringLength(50)]
        public string job_status_set_desc { get; set; }
    }
    public class JobStatusSetNew
    {
        [StringLength(50)]
        public string job_status_set { get; set; }
        [StringLength(50)]
        public string job_status_set_desc { get; set; }
    }            
    public class JobStatusSetSearch : SearchParameter
    {
        public string Search_job_status_set { get; set; }
        public string Search_job_status_set_desc { get; set; }
        
    }
}