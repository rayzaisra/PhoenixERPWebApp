using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("commission",Schema ="hr")]
    public partial class Commission : GenericEntity<string>
    {
        public int? amount { get; set; }
        public string commission_description { get; set; }
        public DateTime? date_commission { get; set; }
        public long? employee_basic_info_id { get; set; }
        public string notes { get; set; }
        public string title { get; set; }
    }
}