using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TravelDto
    {
        [Required]
            public string Id { get; set; }
        public int? actual_travel_budget { get; set; }
        public int? expected_travel_budget { get; set; }
        [StringLength(150)]
        public string purpose_of_visit { get; set; }
        [StringLength(2147483647)]
        public string travel_description { get; set; }
        public DateTime? travel_end_date { get; set; }
        public DateTime? travel_start_date { get; set; }
    }
    public class TravelNew
    {
        public int? actual_travel_budget { get; set; }
        public int? expected_travel_budget { get; set; }
        [StringLength(150)]
        public string purpose_of_visit { get; set; }
        [StringLength(2147483647)]
        public string travel_description { get; set; }
        public DateTime? travel_end_date { get; set; }
        public DateTime? travel_start_date { get; set; }
    }            
    public class TravelSearch : SearchParameter
    {
        public int? Search_actual_travel_budget { get; set; }
        public int? Search_expected_travel_budget { get; set; }
        public string Search_purpose_of_visit { get; set; }
        public string Search_travel_description { get; set; }
        public DateTime? Search_travel_end_date { get; set; }
        public DateTime? Search_travel_start_date { get; set; }
        
    }
}