using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class ActivityDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string code { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string description { get; set; }
        [StringLength(50)]
        [Required]
        public string type { get; set; }
    }
    public class ActivityNew
    {
        [StringLength(50)]
        [Required]
        public string code { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string description { get; set; }
        [StringLength(50)]
        [Required]
        public string type { get; set; }
    }            
    public class ActivitySearch : SearchParameter
    {
        public string Search_code { get; set; }
        public string Search_description { get; set; }
        public string Search_type { get; set; }
        
    }
}