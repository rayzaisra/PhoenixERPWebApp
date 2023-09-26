using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("task_status",Schema ="pm")]
    public partial class TaskStatus : GenericEntity<string>
    {
        public string name { get; set; }
    }
}