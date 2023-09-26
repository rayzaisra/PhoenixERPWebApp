using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class LevelPositionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string position_level { get; set; }
    }
    public class LevelPositionNew
    {
        [StringLength(50)]
        [Required]
        public string position_level { get; set; }
    }            
    public class LevelPositionSearch : SearchParameter
    {
        public string Search_position_level { get; set; }
        
    }
}