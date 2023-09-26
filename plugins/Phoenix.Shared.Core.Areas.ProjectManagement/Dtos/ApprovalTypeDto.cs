using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ApprovalTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string approval_type { get; set; }
    }
    public class ApprovalTypeNew
    {
        [StringLength(50)]
        public string approval_type { get; set; }
    }            
    public class ApprovalTypeSearch : SearchParameter
    {
        public string Search_approval_type { get; set; }
        
    }
}