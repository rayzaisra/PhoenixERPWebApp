using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CandidateRecruitmentSignContractDto
    {
        [Required]
            public string Id { get; set; }
        public string candidate_recruitment_id {get;set;}
		public string filemaster_id {get;set;}
		public DateTime? created_on {get;set;}
        public string remarks { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
    public class CandidateRecruitmentSignContractNew
    {
		public string Id { get; set; }
        public string candidate_recruitment_id {get;set;}
		public string filemaster_id {get;set;}
		public DateTime? created_on {get;set;}
        public string remarks { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }            
    public class CandidateRecruitmentSignContractSearch : SearchParameter
    {
        
        
    }
}