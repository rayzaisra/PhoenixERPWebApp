using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("bonus",Schema ="hr")]
    public partial class Bonus : GenericEntity<string>
    {
        public int? amount { get; set; }
        public string bonus_description { get; set; }
        public DateTime? date_bonus { get; set; }
        public string employee_basic_info_id { get; set; }
        public string notes { get; set; }
        public string title { get; set; }
    }
}