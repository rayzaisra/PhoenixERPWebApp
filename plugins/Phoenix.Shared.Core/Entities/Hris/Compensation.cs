using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("compensation",Schema ="hr")]
    public partial class Compensation : GenericEntity<string>
    {
        public string compensation_id { get; set; }
        public DateTime? date_compensation { get; set; }
        public long? employee_basic_info_id { get; set; }
        public DateTime? end_compensation { get; set; }
        public string type { get; set; }
        public int? value { get; set; }
    }
}