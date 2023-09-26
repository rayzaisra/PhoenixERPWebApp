using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class CentralResourceAssigmentDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string approval_status_id { get; set; }
        [StringLength(50)]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string jobnumber { get; set; }
    }
    public class CentralResourceAssigmentNew
    {
        [StringLength(50)]
        public string approval_status_id { get; set; }
        [StringLength(50)]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string jobnumber { get; set; }
    }            
    public class CentralResourceAssigmentSearch : SearchParameter
    {
        public string Search_approval_status_id { get; set; }
        public string Search_central_resource_id { get; set; }
        public string Search_client_brief_id { get; set; }
        public string Search_jobnumber { get; set; }
        
    }
}