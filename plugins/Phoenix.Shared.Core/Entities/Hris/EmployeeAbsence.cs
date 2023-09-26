using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_absence",Schema ="hr")]
    public partial class EmployeeAbsence : GenericEntity<string>
    {
        public DateTime? date_absence { get; set; }
        public string employee_basic_info_id { get; set; }
        public string note { get; set; }
        public DateTime? time_back { get; set; }
        public DateTime? time_come { get; set; }
    }
}