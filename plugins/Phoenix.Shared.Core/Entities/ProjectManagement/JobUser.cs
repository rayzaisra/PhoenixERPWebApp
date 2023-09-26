using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_user",Schema="pm")]
    public partial class JobUser : GenericEntity<string>
    {
        public string job_detail_id { get; set; }
        public string user_id { get; set; }
    }
}
