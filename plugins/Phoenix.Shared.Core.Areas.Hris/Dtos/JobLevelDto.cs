using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobLevelDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string job_code { get; set; }
        [StringLength(100)]
        public string job_name { get; set; }
    }
    public class JobLevelNew
    {
        [StringLength(50)]
        public string job_code { get; set; }
        [StringLength(100)]
        public string job_name { get; set; }
    }            
    public class JobLevelSearch : SearchParameter
    {
        public string Search_job_code { get; set; }
        public string Search_job_name { get; set; }
        
    }
}