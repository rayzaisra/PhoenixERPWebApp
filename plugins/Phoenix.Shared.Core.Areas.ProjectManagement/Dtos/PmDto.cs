using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PmDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
    }
    public class PmNew
    {
        [StringLength(50)]
        public string user_id { get; set; }
    }            
    public class PmSearch : SearchParameter
    {
        public string Search_user_id { get; set; }
        
    }
}