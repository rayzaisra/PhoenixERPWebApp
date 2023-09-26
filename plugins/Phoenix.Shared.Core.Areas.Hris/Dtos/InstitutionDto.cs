using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class InstitutionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string institution_address { get; set; }
        [StringLength(50)]
        [Required]
        public string institution_name { get; set; }
        public string institution_type { get; set; }
    }
    public class InstitutionNew
    {
        [StringLength(2147483647)]
        [Required]
        public string institution_address { get; set; }
        [StringLength(50)]
        [Required]
        public string institution_name { get; set; }
        public string institution_type { get; set; }
    }            
    public class InstitutionSearch : SearchParameter
    {
        public string Search_institution_address { get; set; }
        public string Search_institution_type { get; set; }
        public string Search_institution_name { get; set; }
        
    }
}