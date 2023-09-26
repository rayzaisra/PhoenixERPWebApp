using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("approval_type",Schema ="pm")]
    public partial class ApprovalType : GenericEntity<string>
    {
        public string approval_type { get; set; }
    }
}