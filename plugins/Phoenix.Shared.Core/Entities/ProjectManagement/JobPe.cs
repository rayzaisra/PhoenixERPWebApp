using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_pe",Schema ="pm")]
    public partial class JobPe : GenericEntity<string>
    {
        public string approval_id { get; set; }
        public string ca_number { get; set; }
        public string child_pce_number { get; set; }
        public string detail { get; set; }
        public DateTime? due_date { get; set; }
        public string file_id { get; set; }
        public string job_detail_id { get; set; }
        public string job_pa_id { get; set; }
        public double? margin { get; set; }
        public decimal? pe_cost { get; set; }
        public DateTime? pe_date { get; set; }
        public int? pe_line_item { get; set; }
        public int? pe_number { get; set; }
        public string proc_number { get; set; }
        public string submitted_by { get; set; }
    }
}