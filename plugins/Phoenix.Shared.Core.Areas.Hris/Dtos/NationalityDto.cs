using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class NationalityDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string nationality_name { get; set; }
    }
    public class NationalityNew
    {
        [StringLength(50)]
        [Required]
        public string nationality_name { get; set; }
    }            
    public class NationalitySearch : SearchParameter
    {
        public string Search_nationality_name { get; set; }
        
    }
}