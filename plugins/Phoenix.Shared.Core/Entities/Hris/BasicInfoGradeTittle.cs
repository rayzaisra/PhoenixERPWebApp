using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("basic_info_grade_tittle",Schema ="hr")]
    public partial class BasicInfoGradeTittle : GenericEntity<string>
    {
        public string employee_basic_info_id { get; set; }
        public string job_grade_id { get; set; }
        public string job_tittle_id { get; set; }
    }
}