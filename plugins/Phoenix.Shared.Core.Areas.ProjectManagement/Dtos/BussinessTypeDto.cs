using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class BussinessTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string propose_type { get; set; }
    }
    public class BussinessTypeNew
    {
        [StringLength(50)]
        public string propose_type { get; set; }
    }            
    public class BussinessTypeSearch : SearchParameter
    {
        public string Search_propose_type { get; set; }
        
    }
}