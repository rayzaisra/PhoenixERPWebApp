using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class LocationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string client_brief_id { get; set; }
        [Required]
        public int? lattitude { get; set; }
        [StringLength(100)]
        [Required]
        public string location_name { get; set; }
        [Required]
        public int? longitude { get; set; }
    }
    public class LocationNew
    {
        [StringLength(50)]
        [Required]
        public string client_brief_id { get; set; }
        [Required]
        public int? lattitude { get; set; }
        [StringLength(100)]
        [Required]
        public string location_name { get; set; }
        [Required]
        public int? longitude { get; set; }
    }            
    public class LocationSearch : SearchParameter
    {
        public string Search_client_brief_id { get; set; }
        public int? Search_lattitude { get; set; }
        public string Search_location_name { get; set; }
        public int? Search_longitude { get; set; }
        
    }
}