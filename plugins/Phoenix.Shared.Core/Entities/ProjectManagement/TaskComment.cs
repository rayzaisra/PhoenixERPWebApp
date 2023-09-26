using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("task_comment",Schema ="pm")]
    public partial class TaskComment : GenericEntity<string>
    {
        public string comment { get; set; }
        public DateTime? comment_date { get; set; }
        public string file_id { get; set; }
        public string task_id { get; set; }
        public string user_id { get; set; }
    }
}