using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_test",Schema ="hr")]
    public partial class JobTest : GenericEntity<string>
    {
        public string job_posting_requisition_id { get; set; }
        public string job_test_description { get; set; }
        public string notes { get; set; }
        public string test_title { get; set; }
    }
}