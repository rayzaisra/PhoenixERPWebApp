using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_workflow",Schema ="pm")]
    public partial class JobWorkflow : GenericEntity<string>
    {
        public string description { get; set; }
        public string workflow_name { get; set; }

    }
}