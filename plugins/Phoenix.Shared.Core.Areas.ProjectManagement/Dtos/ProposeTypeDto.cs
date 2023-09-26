using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ProposeTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string name { get; set; }
    }
    public class ProposeTypeNew
    {
        [StringLength(50)]
        public string name { get; set; }
    }            
    public class ProposeTypeSearch : SearchParameter
    {
        public string Search_name { get; set; }
        
    }
}