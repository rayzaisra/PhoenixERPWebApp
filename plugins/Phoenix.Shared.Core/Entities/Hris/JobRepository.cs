using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_repository",Schema ="hr")]
    public partial class JobRepository : GenericEntity<string>
    {
        public DateTime? date { get; set; }
        public string external_id { get; set; }
        public string job_posting_requisition_id { get; set; }
        public string pe_file_id { get; set; }
        public int? remark { get; set; }
        public int? version { get; set; }
    }
}