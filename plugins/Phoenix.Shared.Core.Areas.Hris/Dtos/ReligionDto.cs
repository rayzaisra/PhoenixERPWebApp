using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class ReligionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string religion_name { get; set; }
    }
    public class ReligionNew
    {
        [StringLength(50)]
        [Required]
        public string religion_name { get; set; }
    }            
    public class ReligionSearch : SearchParameter
    {
        public string Search_religion_name { get; set; }
        
    }
}