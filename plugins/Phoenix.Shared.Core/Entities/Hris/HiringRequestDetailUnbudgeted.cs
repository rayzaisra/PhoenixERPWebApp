using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("hiring_request_detail_unbudgeted",Schema ="hr")]
    public partial class HiringRequestDetailUnbudgeted : GenericEntity<string>
    {
        public string employment_status { get; set; }
        public double? mpp_budged_id { get; set; }
        public string ttf { get; set; }
    }
}