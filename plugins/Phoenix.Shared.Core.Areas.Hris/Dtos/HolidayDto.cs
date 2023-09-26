using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class HolidayDto
    {
        [Required]
            public string Id { get; set; }
        [Required]
        public DateTime? date { get; set; }
        [StringLength(100)]
        [Required]
        public string name { get; set; }
    }
    public class HolidayNew
    {
        [Required]
        public DateTime? date { get; set; }
        [StringLength(100)]
        [Required]
        public string name { get; set; }
    }            
    public class HolidaySearch : SearchParameter
    {
        public DateTime? Search_date { get; set; }
        public string Search_name { get; set; }
        
    }
}