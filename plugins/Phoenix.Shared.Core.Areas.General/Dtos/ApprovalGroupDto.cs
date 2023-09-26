using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApprovalGroupDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(400)]
        [Required]
        public string group_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }
    public class ApprovalGroupNew
    {
        [StringLength(400)]
        [Required]
        public string group_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }            
    public class ApprovalGroupSearch : SearchParameter
    {
        public string Search_group_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}