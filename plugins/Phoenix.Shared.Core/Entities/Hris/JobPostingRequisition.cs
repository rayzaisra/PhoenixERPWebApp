using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_posting_requisition",Schema ="hr")]
    public partial class JobPostingRequisition : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public int? candidate_age_range_end { get; set; }
        public int? candidate_age_range_start { get; set; }
        public string candidate_experience { get; set; }
        public string candidate_qualification { get; set; }
        public string central_resource_id { get; set; }
        public string client_brief_id { get; set; }
        public string code { get; set; }
        public string job_post_description { get; set; }
        public string job_title { get; set; }
        public string notes { get; set; }
        public int? number_of_position { get; set; }
        public string reg_code { get; set; }
        public int? salary_end { get; set; }
        public int? salary_start { get; set; }
        public string status { get; set; }
    }
}