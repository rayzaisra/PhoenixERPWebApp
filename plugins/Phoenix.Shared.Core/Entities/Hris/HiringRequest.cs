using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("hiring_request",Schema ="hr")]
    public partial class HiringRequest : GenericEntity<string>
    {
        public string approval_id { get; set; }
        public string basic_info_grade_tittle { get; set; }
        public string detail_candidate_qualification { get; set; }
        public string code { get; set; }
        public DateTime? date_fulfilment { get; set; }
        public string requester_employee_basic_info_id { get; set; }
        public string employment_status { get; set; }
        public decimal? mpp_budged { get; set; }
        public string mpp_id { get; set; }
        public string ttf { get; set; }
        public string hiring_request_type_id { get; set; }
        public string job_grade_id { get; set; }
        public string job_title_id { get; set; }
        public string replacement_employee_basic_info_id { get; set; }
        public string detail_candidate_responsibilites { get; set; }
        public string budget_souce { get; set; }
        public string status { get; set; }
    }
}