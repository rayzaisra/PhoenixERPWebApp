using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_interviewer",Schema ="hr")]
    public partial class JobInterviewer : GenericEntity<string>
    {
        public string employee_basic_info_id { get; set; }
    }
}