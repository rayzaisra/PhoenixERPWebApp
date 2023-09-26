using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_interview",Schema ="hr")]
    public partial class CandidateRecruitmentInterview : GenericEntity<string>
    {
		
        public string candidate_recruitment_id { get; set; }
        public string job_vacancy_id { get; set; }
        public DateTime? date_interview { get; set; }
        public string interviewer { get; set; }
        public Int32 time_from_hour { get; set; }
        public Int32 time_from_minutes { get; set; }
        public Int32 time_to_hour { get; set; }
        public Int32 time_to_minutes { get; set; }
        public string Interview_to { get; set; }
        public string Interview_from { get; set; }
        public string master_location_id { get; set; }
        public string content { get; set; }
        public string subject { get; set; }
    }
}