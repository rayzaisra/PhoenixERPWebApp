using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("hiring_applicant",Schema ="hr")]
    public partial class HiringApplicant : GenericEntity<string>
    {
        public string job_vacancy_id { get; set; }
        public string candidate_recruitment_id { get; set; }
    }
}