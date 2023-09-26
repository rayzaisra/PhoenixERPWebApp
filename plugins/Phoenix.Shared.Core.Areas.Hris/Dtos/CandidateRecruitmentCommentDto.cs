using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CandidateRecruitmentCommentDto
    {
        [Required]
            public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public string comment_message { get; set; }
        public string name_employee { get; set; }
        public DateTime? created_on { get; set; }
    }
    public class CandidateRecruitmentCommentNew
    {
		public string Id { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public string comment_message { get; set; }
    }            
    public class CandidateRecruitmentCommentSearch : SearchParameter
    {
        public string search_id { get; set; }
        public string search_candidate_id { get; set; }

    }
}