using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("timesheet_nonjob",Schema ="pm")]
    public partial class TimesheetNonJob : GenericEntity<string>
    {
        public string code { get; set; }
        public string description { get; set; }
        public string business_unit_id { get; set; }
        public string detail { get; set; }
        public string name { get; set; }
        public int revision { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}