using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("dam", Schema = "pm")]
    public partial class Dam : GenericEntity<string>
    {
        public string job_detail_id { get; set; }
        public string dam_type_id { get; set; }
        public string file_id { get; set; }
        public string notes { get; set; }
        public string sub_task_id { get; set; }
        public string task_id { get; set; }
        public string dam { get; set; }
    }
}