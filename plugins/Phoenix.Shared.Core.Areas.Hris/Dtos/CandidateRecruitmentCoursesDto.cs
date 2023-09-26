using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CandidateRecruitmentCoursesDto
    {
        [Required]
            public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string course_name { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string remarks { get; set; }
    }
    public class CandidateRecruitmentCoursesNew
    {
        
        public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        [Required]
        public string course_name { get; set; }
        [Required]
        public string year_from { get; set; }
        [Required]
        public string year_to { get; set; }
        public string remarks { get; set; }
    }            
    public class CandidateRecruitmentCoursesSearch : SearchParameter
    {
        
        public string Search_candidate_recruitment_id { get; set; }
    }
}