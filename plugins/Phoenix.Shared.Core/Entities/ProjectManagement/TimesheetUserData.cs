using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("timesheet_user_data", Schema = "pm")]
    public partial class TimesheetUserData : GenericEntity<string>
    {
        
        public string task_id { get; set; }

        public int? timesheet_hour { get; set; }

        public DateTime timesheet_date { get; set; }

        public string user_id { get; set; }
        public string timesheet_userid { get; set; }
        public string remarks { get; set; }

    }
}
