using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class WorkingEffectiveDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string from_time { get; set; }
        [StringLength(50)]
        [Required]
        public string to_time { get; set; }
    }
    public class WorkingEffectiveNew
    {
        [StringLength(50)]
        [Required]
        public string from_time { get; set; }
        [StringLength(50)]
        [Required]
        public string to_time { get; set; }
    }            
    public class WorkingEffectiveSearch : SearchParameter
    {
        public string Search_from_time { get; set; }
        public string Search_to_time { get; set; }
        
    }
}