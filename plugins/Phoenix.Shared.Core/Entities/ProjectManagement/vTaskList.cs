using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("v_tasklist",Schema ="pm")]
    public partial class vTaskList
    {
        public string current_id { get; set; }
        public string job_id { get; set; }
        public string task_id { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string parent { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public double  progress { get; set; }
        public string priority { get; set; }
        public string description { get; set; }
        public int hour { get; set; }
        public string users { get; set; }
    }
}