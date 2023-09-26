using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class RateCardCodeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        [StringLength(250)]
        public string description { get; set; }
    }
    public class RateCardCodeNew
    {
        [StringLength(50)]
        public string code { get; set; }
        [StringLength(250)]
        public string description { get; set; }
    }            
    public class RateCardCodeSearch : SearchParameter
    {
        public string Search_code { get; set; }
        public string Search_description { get; set; }
        
    }
}