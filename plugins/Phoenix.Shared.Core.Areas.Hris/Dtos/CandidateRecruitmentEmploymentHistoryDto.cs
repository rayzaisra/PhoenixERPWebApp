using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CandidateRecruitmentEmploymentHistoryDto
    {
        [Required]
            public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
        public string company_phone { get; set; }
        public string year_from { get; set; }
        public string year_to { get; set; }
        public string position { get; set; }
        public string main_responsibilities { get; set; }
        public DateTime? created_on { get; set; }
        public string achievements { get; set; }
        
    }
    public class CandidateRecruitmentEmploymentHistoryNew
    {
        
        public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        [Required]
        public string company_name { get; set; }
        [Required]
        public string company_address { get; set; }
        [Required]
        public string company_phone { get; set; }
        [Required]
        public string year_from { get; set; }
        [Required]
        public string year_to { get; set; }
        [Required]
        public string position { get; set; }
        public string main_responsibilities { get; set; }
        public DateTime? created_on { get; set; }
        public string achievements { get; set; }
    }            
    public class CandidateRecruitmentEmploymentHistorySearch : SearchParameter
    {
        public string Search_candidate_recruitment_id { get; set; }


    }
}