using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_type",Schema ="pm")]
    public partial class JobType : GenericEntity<string>
    {
        
        public string description { get; set; }
        public string name { get; set; }
    }
}