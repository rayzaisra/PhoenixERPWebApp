using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_training",Schema ="hr")]
    public partial class EmployeeTraining : GenericEntity<string>
    {
        public string address { get; set; }
        public string description { get; set; }
        public string employee_basic_info_id { get; set; }
        public string end_training { get; set; }
        public DateTime? expired_certificate { get; set; }
        public string filemaster_id { get; set; }
        public string institution_id { get; set; }
        public string name_training { get; set; }
        public string no_certification { get; set; }
        public string start_training { get; set; }
    }
}