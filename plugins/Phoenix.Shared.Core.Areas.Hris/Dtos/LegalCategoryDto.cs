using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class LegalCategoryDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string category { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string description { get; set; }
    }
    public class LegalCategoryNew
    {
        [StringLength(50)]
        [Required]
        public string category { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string description { get; set; }
    }            
    public class LegalCategorySearch : SearchParameter
    {
        public string Search_category { get; set; }
        public string Search_description { get; set; }
        
    }
}