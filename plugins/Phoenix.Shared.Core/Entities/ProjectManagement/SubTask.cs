using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("sub_task",Schema ="pm")]
    public partial class SubTask : GenericEntity<string>
    {
        public string attachment { get; set; }
        public string description { get; set; }
        public DateTime? due_date { get; set; }
        public int? hour { get; set; }
        public Single? percentage { get; set; }
        public DateTime? start_date { get; set; }
        public string sub_task { get; set; }
        public string task_id { get; set; }
        public string task_status_id { get; set; }

    }
}