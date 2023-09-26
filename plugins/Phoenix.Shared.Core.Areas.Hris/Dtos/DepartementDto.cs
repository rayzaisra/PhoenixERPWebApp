using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class DepartementDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string departement_code { get; set; }
        [StringLength(100)]
        public string departement_name { get; set; }
        [StringLength(100)]
        public string divisionid { get; set; }
    }
    public class DepartementNew
    {
        [StringLength(50)]
        public string departement_code { get; set; }
        [StringLength(100)]
        public string departement_name { get; set; }
        [StringLength(100)]
        public string divisionid { get; set; }
    }            
    public class DepartementSearch : SearchParameter
    {
        public string Search_departement_code { get; set; }
        public string Search_departement_name { get; set; }
        public string Search_divisionid { get; set; }
        
    }
}