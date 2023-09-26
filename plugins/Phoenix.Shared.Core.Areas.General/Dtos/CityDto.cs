using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CityDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(5)]
        public string city_code { get; set; }
        [StringLength(400)]
        public string city_name { get; set; }
        [StringLength(50)]
        public string country_id { get; set; }
    }
    public class CityNew
    {
        [StringLength(5)]
        public string city_code { get; set; }
        [StringLength(400)]
        public string city_name { get; set; }
        [StringLength(50)]
        public string country_id { get; set; }
    }            
    public class CitySearch : SearchParameter
    {
        public string Search_city_code { get; set; }
        public string Search_city_name { get; set; }
        public string Search_country_id { get; set; }
        
    }
}