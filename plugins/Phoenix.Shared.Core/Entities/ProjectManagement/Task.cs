using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("task",Schema ="pm")]
    public partial class Task : GenericEntity<string>
    {
        public string central_resources_id { get; set; }
        public DateTime? deadline_task { get; set; }
        public string description { get; set; }
        public string job_number { get; set; }
        public string job_posting_requisition_id { get; set; }
        public DateTime? start_task { get; set; }
        public string task_name { get; set; }
        public string task_priority_id { get; set; }
        public string task_status_id { get; set; }
    }
}