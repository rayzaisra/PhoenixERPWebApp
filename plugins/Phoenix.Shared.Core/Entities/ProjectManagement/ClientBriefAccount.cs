using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_brief_account",Schema ="pm")]
    public partial class ClientBriefAccount : GenericEntity<string>
    {
        public string account_management_id { get; set; }
        public string approval_status_id { get; set; }
        public string client_brief_id { get; set; }
        public string job_posting_requisition_id { get; set; }
    }
}