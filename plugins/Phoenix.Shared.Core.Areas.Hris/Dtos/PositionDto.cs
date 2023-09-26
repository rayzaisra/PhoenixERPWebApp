using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class PositionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string central_resource_id { get; set; }
        public int? head { get; set; }
        [StringLength(50)]
        [Required]
        public string level_position_id { get; set; }
        [StringLength(50)]
        [Required]
        public string position_name { get; set; }
    }
    public class PositionNew
    {
        [StringLength(50)]
        public string central_resource_id { get; set; }
        public int? head { get; set; }
        [StringLength(50)]
        [Required]
        public string level_position_id { get; set; }
        [StringLength(50)]
        [Required]
        public string position_name { get; set; }
    }            
    public class PositionSearch : SearchParameter
    {
        public string Search_central_resource_id { get; set; }
        public int? Search_head { get; set; }
        public string Search_level_position_id { get; set; }
        public string Search_position_name { get; set; }
        
    }
}