using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class SalaryComponentDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string code { get; set; }
        [StringLength(50)]
        [Required]
        public string salary_component { get; set; }
        [StringLength(50)]
        public string description { get; set; }
    }
    public class SalaryComponentNew
    {
        [StringLength(50)]
        [Required]
        public string code { get; set; }
        [StringLength(50)]
        [Required]
        public string salary_component { get; set; }
        public string description { get; set; }
    }            
    public class SalaryComponentSearch : SearchParameter
    {
        public string Search_code { get; set; }
        public string Search_salary_component { get; set; }
        
    }
}