using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("information",Schema ="hr")]
    public partial class Information : GenericEntity<string>
    {
        public string body { get; set; }
        public DateTime? date_information { get; set; }
        public int? flag_active { get; set; }
        public string title { get; set; }
    }
}