using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_out",Schema ="hr")]
    public partial class EmployeeOut : GenericEntity<string>
    {
        public DateTime? date_out { get; set; }
        public string employee_basic_info_id { get; set; }
        public string reason { get; set; }
        public string time_comeback { get; set; }
        public string time_out { get; set; }
        public string type { get; set; }
    }
}