using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_training",Schema ="hr")]
    public partial class CandidateRecruitmentTraining : GenericEntity<string>
    {
		
        public string candidate_recruitment_id { get; set; }
        public string training_name { get; set; }
        public string status { get; set; }
        public string score { get; set; }
        public string job_vacancy_id { get; set; }
    }
}