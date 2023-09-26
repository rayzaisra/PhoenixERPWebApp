using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("termination",Schema ="hr")]
    public partial class Termination : GenericEntity<string>
    {
        public string employee_basic_info_id { get; set; }
        public string reason { get; set; }
        public DateTime? termination_date { get; set; }
    }
}