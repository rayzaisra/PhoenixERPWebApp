using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class HiringApplicantDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string hiring_request_id { get; set; }
        public string candidate_recruitment_id { get; set; }
    }
    public class HiringApplicantNew
    {
        [StringLength(50)]
        public string hiring_request_id { get; set; }
    }            
    public class HiringApplicantSearch : SearchParameter
    {
        public string Search_hiring_request_id { get; set; }
        
    }
}