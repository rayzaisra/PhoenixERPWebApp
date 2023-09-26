using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("overtime",Schema ="hr")]
    public partial class Overtime : GenericEntity<string>
    {
        public int? amount { get; set; }
        public string approval { get; set; }
        public string approval_by { get; set; }
        public DateTime? approval_date { get; set; }
        public string employee_basic_info_id { get; set; }
        public string hour_overtime { get; set; }
        public DateTime? last_update { get; set; }
        public DateTime? overtime_date { get; set; }
    }
}