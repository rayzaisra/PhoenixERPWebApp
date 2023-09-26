using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobWorkflowMasterDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(50)]
        public string next_workflow { get; set; }
        [StringLength(250)]
        public string screen_flow { get; set; }
        [StringLength(250)]
        public string status { get; set; }
    }
    public class JobWorkflowMasterNew
    {
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(50)]
        public string next_workflow { get; set; }
        [StringLength(250)]
        public string screen_flow { get; set; }
        [StringLength(250)]
        public string status { get; set; }
    }            
    public class JobWorkflowMasterSearch : SearchParameter
    {
        public string Search_description { get; set; }
        public string Search_next_workflow { get; set; }
        public string Search_screen_flow { get; set; }
        public string Search_status { get; set; }
        
    }
}