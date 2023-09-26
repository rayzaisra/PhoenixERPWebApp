using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("task_priority",Schema ="pm")]
    public partial class TaskPriority : GenericEntity<string>
    {
        public string name { get; set; }
    }
}