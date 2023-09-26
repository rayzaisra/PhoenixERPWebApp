using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MppStatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string name { get; set; }
    }
    public class MppStatusNew
    {
        [StringLength(100)]
        public string name { get; set; }
    }            
    public class MppStatusSearch : SearchParameter
    {
        public string Search_name { get; set; }
        
    }
}