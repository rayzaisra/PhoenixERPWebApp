using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("premiaskes",Schema ="hr")]
    public partial class Premiaskes : GenericEntity<string>
    {
        public long? employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? start_date { get; set; }
        public int? value { get; set; }
    }
}