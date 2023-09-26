using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobStatusDetailDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(150)]
        public string name { get; set; }
    }
    public class JobStatusDetailNew
    {
        [StringLength(150)]
        public string name { get; set; }
    }            
    public class JobStatusDetailSearch : SearchParameter
    {
        public string Search_name { get; set; }
        
    }
}