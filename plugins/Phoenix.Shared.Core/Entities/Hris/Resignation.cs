using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("resignation",Schema ="hr")]
    public partial class Resignation : GenericEntity<string>
    {
        public string employee_basic_info_id { get; set; }
        public string notes { get; set; }
        public DateTime? notice_date { get; set; }
        public DateTime? resignation_date { get; set; }
        public string resignation_reason { get; set; }
    }
}