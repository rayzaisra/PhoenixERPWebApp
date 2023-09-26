using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobIntervieweesDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string candidate_recruitment_id { get; set; }
    }
    public class JobIntervieweesNew
    {
        [StringLength(50)]
        public string candidate_recruitment_id { get; set; }
    }            
    public class JobIntervieweesSearch : SearchParameter
    {
        public string Search_candidate_recruitment_id { get; set; }
        
    }
}