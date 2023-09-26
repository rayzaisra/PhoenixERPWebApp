using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("achievement",Schema ="hr")]
    public partial class Achievement : GenericEntity<string>
    {
        public DateTime? achievement_date { get; set; }
        public string achievement_description { get; set; }
        public string achievement_title { get; set; }
        public long? employee_basic_info_id { get; set; }
        public string notes { get; set; }
    }
}