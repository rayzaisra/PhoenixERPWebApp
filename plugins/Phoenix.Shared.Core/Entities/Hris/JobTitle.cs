using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_title",Schema ="hr")]
    public partial class JobTitle : GenericEntity<string>
    {
       
        public string title_code { get; set; }
        public string title_name { get; set; }
    }
}