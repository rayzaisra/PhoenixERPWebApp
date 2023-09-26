using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("task_timesheet",Schema ="pm")]
    public partial class TaskTimesheet : GenericEntity<string>
    {
        public DateTime? date_worksheet { get; set; }
        public DateTime? end_time { get; set; }
        public string notes { get; set; }
        public DateTime? start_time { get; set; }
        public string task { get; set; }
        public string sub_task_id { get; set; }
        public string worksheet_description { get; set; }
        public int? working_hour { get; set; }
        public int? working_minutes { get; set; }
        public string user_id { get; set; }

    }
}