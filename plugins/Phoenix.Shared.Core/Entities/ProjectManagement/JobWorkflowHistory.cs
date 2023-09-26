using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_workflow_history",Schema ="pm")]
    public partial class JobWorkflowHistory : GenericEntity<string>
    {
        public string current_status_created_by { get; set; }
        public DateTime? current_status_created_date { get; set; }
        public string current_status_id { get; set; }
        public string job_number { get; set; }
        public string last_status_created_by { get; set; }
        public DateTime? last_status_created_date { get; set; }
        public string last_status_id { get; set; }
        public int? seq { get; set; }
    }
}