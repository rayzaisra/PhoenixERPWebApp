using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_status_detail",Schema ="pm")]
    public partial class JobStatusDetail : GenericEntity<string>
    {
        public string name { get; set; }
    }
}