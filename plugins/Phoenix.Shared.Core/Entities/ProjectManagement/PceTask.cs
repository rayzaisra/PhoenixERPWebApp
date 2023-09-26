using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pce_task",Schema ="pm")]
    public partial class PceTask : GenericEntity<string>
    {
        public decimal? price { get; set; }
        public string task_name { get; set; }
        public string departement { get; set; }
 
        public int quantity { get; set; }
        public string unit { get; set; }
        public string detail { get; set; }
        public decimal? total { get; set; }
        public string pce_id { get; set; }
    }
}