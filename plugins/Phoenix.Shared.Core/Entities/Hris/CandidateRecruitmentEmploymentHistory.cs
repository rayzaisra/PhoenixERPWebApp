using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_employment_history",Schema ="hr")]
    public partial class CandidateRecruitmentEmploymentHistory : GenericEntity<string>
    {
        public string candidate_recruitment_id {get;set;}
		public string company_name  {get;set;}
		public string company_address {get;set;}
		public string company_phone {get;set;}
		public string year_from {get;set;}
        public string year_to { get; set; }
        public string position { get; set; }
		public string main_responsibilities { get; set; }
        public string achievements { get; set; }
    }
}