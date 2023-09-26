using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(50)]
        public string name { get; set; }
    }
    public class JobTypeNew
    {
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(50)]
        public string name { get; set; }
    }            
    public class JobTypeSearch : SearchParameter
    {
        public string Search_description { get; set; }
        public string Search_name { get; set; }
        
    }
}