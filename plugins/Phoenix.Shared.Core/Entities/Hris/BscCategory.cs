using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("bsc_category",Schema ="hr")]
    public partial class BscCategory : GenericEntity<string>
    {
        public string bsc_category { get; set; }
        public string bsc_description { get; set; }
        public int? status_id { get; set; }
    }
}