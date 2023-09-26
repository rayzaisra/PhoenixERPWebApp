using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("approval_detail",Schema ="dbo")]
    public partial class ApprovalDetail : GenericEntity<string>
    {
        public string approval_id { get; set; }
        public int? approval_level { get; set; }
        public string approval_status_id { get; set; }
        public string approver_id { get; set; }
        public DateTime? expiry_datetime { get; set; }
        public bool is_auto_approved { get; set; }
        public string previous_approval { get; set; }
    }
}