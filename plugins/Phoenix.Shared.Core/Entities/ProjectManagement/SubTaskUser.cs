using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("sub_task_user", Schema = "pm")]
    public partial class SubTaskUser : GenericEntity<string>
    {
        public string user_id { get; set; }
        public string sub_task_id { get; set; }
    }
}


