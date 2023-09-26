using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class UserRoleDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_user_id { get; set; }
    }
    public class UserRoleNew
    {
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_user_id { get; set; }
    }            
    public class UserRoleSearch : SearchParameter
    {
        public string Search_application_role_id { get; set; }
        public string Search_application_user_id { get; set; }
        
    }
}