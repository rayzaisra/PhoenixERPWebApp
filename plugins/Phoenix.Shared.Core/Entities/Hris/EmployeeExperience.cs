using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_experience",Schema ="hr")]
    public partial class EmployeeExperience : GenericEntity<string>
    {
        public string address { get; set; }
        public string employee_basic_info_id { get; set; }
        public string end_date { get; set; }
        public string company_name { get; set; }
        public string phone { get; set; }
        public string position { get; set; }
        public string remark { get; set; }
        public string start_date { get; set; }
    }
}