using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CandidateRecruitmentTrainingDto
    {
        [Required]
            public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string training_name { get; set; }
        public string status { get; set; }
        public string score { get; set; }
        public string job_vacancy_id { get; set; }
    }
    public class CandidateRecruitmentTrainingNew
    {
		public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string training_name { get; set; }
        public string status { get; set; }
        public string score { get; set; }
        public string job_vacancy_id { get; set; }
    }            
    public class CandidateRecruitmentTrainingSearch : SearchParameter
    {
        public string Search_candidate_recruitment_id { get; set; }

    }
}