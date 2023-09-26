using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployementStatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string employment_status_name { get; set; }
    }
    public class EmployementStatusNew
    {
        [StringLength(100)]
        public string employment_status_name { get; set; }
    }            
    public class EmployementStatusSearch : SearchParameter
    {
        public string Search_employment_status_name { get; set; }
        
    }
}