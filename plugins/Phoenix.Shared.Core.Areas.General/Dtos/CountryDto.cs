using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CountryDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(5)]
        public string country_code { get; set; }
        [StringLength(400)]
        public string country_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }
    public class CountryNew
    {
        [StringLength(5)]
        public string country_code { get; set; }
        [StringLength(400)]
        public string country_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }            
    public class CountrySearch : SearchParameter
    {
        public string Search_country_code { get; set; }
        public string Search_country_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}