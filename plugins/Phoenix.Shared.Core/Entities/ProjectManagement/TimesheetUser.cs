using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("timesheet_user", Schema = "pm")]
    public partial class TimesheetUser : GenericEntity<string>
    {

        public string task_id { get; set; }
        public string tasknj_id { get; set; }
        public string task_name { get; set; }

        public string user_id { get; set; }
        public bool status { get; set; }

        public string job_id { get; set; }
        public string Non_job { get; set; }
        public string autocount_time { get; set; }
        public string remark { get; set; }

        public DateTime start_date_weekly {get; set;}
        public DateTime end_date_weekly { get; set; }
    }
}