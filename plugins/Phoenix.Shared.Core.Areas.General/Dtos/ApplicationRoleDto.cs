using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApplicationRoleDto
    {
        [Required]
            public string Id { get; set; }
        public bool is_default_role { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
        [StringLength(400)]
        [Required]
        public string role_name { get; set; }
    }
    public class ApplicationRoleNew
    {
        public bool is_default_role { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
        [StringLength(400)]
        [Required]
        public string role_name { get; set; }
    }            
    public class ApplicationRoleSearch : SearchParameter
    {
        public bool Search_is_default_role { get; set; }
        public string Search_organization_id { get; set; }
        public string Search_role_name { get; set; }
        
    }
}