using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_courses",Schema ="hr")]
    public partial class CandidateRecruitmentCourses : GenericEntity<string>
    {
        public string candidate_recruitment_id {get;set;}
		public string course_name  {get;set;}
		public string year_from {get;set;}
        public string year_to { get; set; }
        public string remarks {get;set;}
    }
}