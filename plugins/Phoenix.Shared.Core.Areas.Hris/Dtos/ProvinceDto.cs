using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class ProvinceDto
    {
        [Required]
            public string Id { get; set; }
        [Required]
        public int? code { get; set; }
        [StringLength(100)]
        [Required]
        public string province { get; set; }
    }
    public class ProvinceNew
    {
        [Required]
        public int? code { get; set; }
        [StringLength(100)]
        [Required]
        public string province { get; set; }
    }            
    public class ProvinceSearch : SearchParameter
    {
        public int? Search_code { get; set; }
        public string Search_province { get; set; }
        
    }
}