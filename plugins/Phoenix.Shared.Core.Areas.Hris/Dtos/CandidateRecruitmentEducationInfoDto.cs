using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CandidateRecruitmentEducationInfoDto
    {
        [Required]
            public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string institution_name { get; set; }
        public string last_education { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string location { get; set; }
        public string major { get; set; }
        public DateTime? created_on { get; set; }
        public string institution_id { get; set; }
        public string education_level_id { get; set; }
    }
    public class CandidateRecruitmentEducationInfoNew
    {
        
        public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string last_education { get; set; }
        public string institution_name { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string location { get; set; }
        public string major { get; set; }
        public DateTime? created_on { get; set; }
        public string institution_id { get; set; }
        public string education_level_id { get; set; }
    }            
    public class CandidateRecruitmentEducationInfoSearch : SearchParameter
    {
        public string Search_candidate_recruitment_id { get; set; }


    }
}