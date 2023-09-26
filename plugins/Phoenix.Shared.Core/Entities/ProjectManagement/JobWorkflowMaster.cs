using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_workflow_master",Schema ="pm")]
    public partial class JobWorkflowMaster : GenericEntity<string>
    {
        public string description { get; set; }
        public int seq { get; set; }
        public string screen_flow { get; set; }
        public string status { get; set; }
        public string job_type { get; set; }
    }
}