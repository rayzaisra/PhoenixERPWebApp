using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("mpp",Schema ="hr")]
    public partial class Mpp : GenericEntity<string>
    {
        public string approval_id { get; set; }
        public string code { get; set; }
        public DateTime? created_date { get; set; }
        public string fiscal_year { get; set; }
        public string status { get; set; }
        public decimal? total_budget { get; set; }
        public DateTime? updated_date { get; set; }
        public string business_unit_id { get; set; }
    }
}