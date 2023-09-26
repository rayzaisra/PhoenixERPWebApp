using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_interview",Schema ="hr")]
    public partial class JobInterview : GenericEntity<string>
    {
        public string from_email { get; set; }
        public string interviewer { get; set; }
        public DateTime? interview_date { get; set; }
        public DateTime? interview_time { get; set; }
        public string job_interview_description { get; set; }
        public string job_posting_requisition_id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string notes { get; set; }
        public string place_of_interview { get; set; }
        public string to_email { get; set; }
    }
}