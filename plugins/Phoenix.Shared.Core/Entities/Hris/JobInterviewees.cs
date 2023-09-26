using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_interviewees",Schema ="hr")]
    public partial class JobInterviewees : GenericEntity<string>
    {
        public string candidate_recruitment_id { get; set; }
    }
}