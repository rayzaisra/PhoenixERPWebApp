using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MarriedStatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string married_name { get; set; }
    }
    public class MarriedStatusNew
    {
        [StringLength(100)]
        public string married_name { get; set; }
    }            
    public class MarriedStatusSearch : SearchParameter
    {
        public string Search_married_name { get; set; }
        
    }
}