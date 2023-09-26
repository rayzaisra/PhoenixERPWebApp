using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("cost_center",Schema ="hr")]
    public partial class CostCenter : GenericEntity<string>
    {
        public string activity_id { get; set; }
        public string cost_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? end_periode { get; set; }
        public string percentage { get; set; }
        public DateTime? periode { get; set; }
    }
}