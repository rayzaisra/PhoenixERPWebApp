using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApplicationMenuCategoryDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string category_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }
    public class ApplicationMenuCategoryNew
    {
        [StringLength(50)]
        public string category_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }            
    public class ApplicationMenuCategorySearch : SearchParameter
    {
        public string Search_category_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}