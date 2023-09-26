using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class RoleAccessMenuDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_menu_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
    }
    public class RoleAccessMenuNew
    {
        [StringLength(50)]
        [Required]
        public string application_menu_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
    }            
    public class RoleAccessMenuSearch : SearchParameter
    {
        public string Search_application_menu_id { get; set; }
        public string Search_application_role_id { get; set; }
        
    }
}