using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class DamTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string name { get; set; }
    }
    public class DamTypeNew
    {
        [StringLength(50)]
        public string name { get; set; }
    }            
    public class DamTypeSearch : SearchParameter
    {
        public string Search_name { get; set; }
        public string Search_category { get; set; }
        
    }
}