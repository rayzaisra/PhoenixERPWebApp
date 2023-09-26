using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_level",Schema ="hr")]
    public partial class JobLevel : GenericEntity<string>
    {
        public string job_code { get; set; }
        public string job_name { get; set; }
    }
}