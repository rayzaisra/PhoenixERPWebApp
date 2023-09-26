using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApplicationMenuDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(450)]
        public string action_url { get; set; }
        [StringLength(50)]
        public string application_menu_category_id { get; set; }
        [StringLength(50)]
        public string application_menu_group_id { get; set; }
        [StringLength(400)]
        public string menu_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
        [StringLength(50)]
        public string parent_menu_id { get; set; }
    }
    public class ApplicationMenuNew
    {
        [StringLength(450)]
        public string action_url { get; set; }
        [StringLength(50)]
        public string application_menu_category_id { get; set; }
        [StringLength(50)]
        public string application_menu_group_id { get; set; }
        [StringLength(400)]
        public string menu_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
        [StringLength(50)]
        public string parent_menu_id { get; set; }
    }            
    public class ApplicationMenuSearch : SearchParameter
    {
        public string Search_action_url { get; set; }
        public string Search_application_menu_category_id { get; set; }
        public string Search_application_menu_group_id { get; set; }
        public string Search_menu_name { get; set; }
        public string Search_organization_id { get; set; }
        public string Search_parent_menu_id { get; set; }
        
    }
}