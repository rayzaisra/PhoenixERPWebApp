using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("candidate_recruitment_signcontract",Schema ="hr")]
    public partial class CandidateRecruitmentSignContract : GenericEntity<string>
    {
        public string candidate_recruitment_id {get;set;}
		public string filemaster_id {get;set;}
        public string remarks { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public double? thp { get; set; }
        public string ptpp { get; set; }
        public string contracttype { get; set; }
        public string comment { get; set; }
        public DateTime? contractSignDate { get; set; }
    }
}