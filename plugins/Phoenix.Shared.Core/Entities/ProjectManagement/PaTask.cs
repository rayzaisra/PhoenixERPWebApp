using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pa_task",Schema ="pm")]
    public partial class PaTask : GenericEntity<string>
    {
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string central_resources_name { get; set; }
        public decimal? margin { get; set; }
        public string job_detail_id { get; set; }
        public bool status { get; set; }
    }
}