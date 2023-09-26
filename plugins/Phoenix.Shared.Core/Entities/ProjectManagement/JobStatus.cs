using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_status",Schema ="pm")]
    public partial class JobStatus : GenericEntity<string>
    {
        public string name { get; set; }
    }
}