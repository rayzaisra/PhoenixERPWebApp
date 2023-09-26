using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_pa",Schema ="pm")]
    public partial class JobPa : GenericEntity<string>
    {
        public string approval_id { get; set; }

        public string department { get; set; }
        public string currency { get; set; }
        public string feetype { get; set; }
        public int? hour_per_day { get; set; }
        public string job_detail_id { get; set; }
        public string mother_pca { get; set; }
        public double? fee_percentage { get; set; }
        public double? feetotal { get; set; }
        public DateTime? pa_date { get; set; }
        public string pa_number { get; set; }
 
        public string rate_card_id { get; set; }
    }
}