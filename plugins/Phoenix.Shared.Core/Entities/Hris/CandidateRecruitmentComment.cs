using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_comment",Schema ="hr")]
    public partial class CandidateRecruitmentComment : GenericEntity<string>
    {
		
        public string candidate_recruitment_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public string comment_message { get; set; }
    }
}