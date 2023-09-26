using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobWorkflowDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        public string workflow_name { get; set; }
    }
    public class JobWorkflowNew
    {
        [StringLength(250)]
        public string description { get; set; }
        public string workflow_name { get; set; }
    }            
    public class JobWorkflowSearch : SearchParameter
    {
        
    }
}