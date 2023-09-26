using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TravelDestinationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string arrangement_type { get; set; }
        [StringLength(150)]
        public string place_of_visit { get; set; }
        [StringLength(50)]
        public string travel_id { get; set; }
        [StringLength(60)]
        public string travel_mode { get; set; }
    }
    public class TravelDestinationNew
    {
        [StringLength(100)]
        public string arrangement_type { get; set; }
        [StringLength(150)]
        public string place_of_visit { get; set; }
        [StringLength(50)]
        public string travel_id { get; set; }
        [StringLength(60)]
        public string travel_mode { get; set; }
    }            
    public class TravelDestinationSearch : SearchParameter
    {
        public string Search_arrangement_type { get; set; }
        public string Search_place_of_visit { get; set; }
        public string Search_travel_id { get; set; }
        public string Search_travel_mode { get; set; }
        
    }
}