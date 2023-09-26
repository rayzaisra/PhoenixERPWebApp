using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_account",Schema ="pm")]
    public partial class JobAccount : GenericEntity<string>
    {
        public string user_id { get; set; }
        public string job_detail_id { get; set; }
		public string act_as { get; set; }
    }
}