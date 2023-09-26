using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pa_detail",Schema ="pm")]
    public partial class PaDetail : GenericEntity<string>
    {
        public string sub_task_id { get; set; }
        public string task_id { get; set; }
        public string category { get; set; }
        public string sub_brand { get; set; }
        public decimal? price_per_quantity { get; set; }
        public decimal? quantity { get; set; }
        public string remarks { get; set; }
        public decimal? ppn { get; set; }
        public decimal? asf { get; set; }
        public decimal? pph { get; set; }
        public string uom { get; set; }
        public string brand { get; set; }
        public string category_name { get; set; }

    }
}