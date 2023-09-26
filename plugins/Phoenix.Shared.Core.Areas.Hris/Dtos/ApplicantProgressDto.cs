using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class ApplicantProgressDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string applicant_progress { get; set; }
    }
    public class ApplicantProgressNew
    {
        [StringLength(50)]
        public string applicant_progress { get; set; }
    }            
    public class ApplicantProgressSearch : SearchParameter
    {
        public string Search_applicant_progress { get; set; }
        
    }
}