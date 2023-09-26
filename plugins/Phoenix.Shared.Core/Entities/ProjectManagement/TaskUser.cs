using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("task_user",Schema ="pm")]
    public partial class TaskUser : GenericEntity<string>
    {
        public string job_posting_requisition_id { get; set; }
        public string task_id { get; set; }
        public string task_user { get; set; }
        public string user_id { get; set; }
    }
}