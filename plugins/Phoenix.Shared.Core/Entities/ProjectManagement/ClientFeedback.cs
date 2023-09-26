using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_feedback", Schema = "pm")]
    public partial class ClientFeedback : GenericEntity<string>
    {
        public string approval_type_id { get; set; }
        public string client_feedback_file_id { get; set; }
        public string job_posting_requisition_id { get; set; }
        public string job_number { get; set; }
    }
}