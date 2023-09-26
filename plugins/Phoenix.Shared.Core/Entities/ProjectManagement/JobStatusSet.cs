using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_status_set",Schema ="pm")]
    public partial class JobStatusSet : GenericEntity<string>
    {
        public string job_status_set { get; set; }
        public string job_status_set_desc { get; set; }
    }
}