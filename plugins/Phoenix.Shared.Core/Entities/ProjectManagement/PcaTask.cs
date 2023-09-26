using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pca_task",Schema ="pm")]
    public partial class PcaTask : GenericEntity<string>
    {
        public decimal? price { get; set; }
        public string task_name { get; set; }
        public string departement { get; set; }
 
        public int quantity { get; set; }
        public string unit { get; set; }
        public string detail { get; set; }
        public decimal? cost { get; set; }
        public int percentage { get; set; }
        public decimal? ppn { get; set; }
        public decimal? asf { get; set; }
        public decimal? pph { get; set; }
        public decimal? margin { get; set; }
        public decimal? total { get; set; }
        public string pca_id { get; set; }
    }
}