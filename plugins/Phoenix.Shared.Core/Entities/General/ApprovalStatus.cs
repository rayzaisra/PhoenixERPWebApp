using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("approval_status",Schema ="dbo")]
    public partial class ApprovalStatus : GenericEntity<string>
    {
        public string approval_group_id { get; set; }
        public string approval_status { get; set; }
        public int? status_level { get; set; }
        public string status_name { get; set; }
    }
}