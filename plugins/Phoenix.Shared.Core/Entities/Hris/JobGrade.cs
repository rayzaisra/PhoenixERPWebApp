using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("job_grade",Schema ="hr")]
    public partial class JobGrade : GenericEntity<string>
    {
        public string grade_code { get; set; }
        public string grade_name { get; set; }
        public string joblevelid { get; set; }
    }
}