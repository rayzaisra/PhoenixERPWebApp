using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_education_info",Schema ="hr")]
    public partial class CandidateRecruitmentEducationInfo : GenericEntity<string>
    {
        public string candidate_recruitment_id {get;set;}
		public string year_from {get;set;}
        public string year_to { get; set; }
        public string location {get;set;}
		public string major {get;set;}
        public string institution_id { get; set; }
        public string education_level_id { get; set; }
    }
}