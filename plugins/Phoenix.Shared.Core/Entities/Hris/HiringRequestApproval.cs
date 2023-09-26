using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("hiring_request_approval", Schema = "hr")]
    public partial class HiringRequestApproval : GenericEntity<string>
    {
        public string hiring_request_id { get; set; }
        public string reason { get; set; }

    }
}
