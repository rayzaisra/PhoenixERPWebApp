using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_emergency_contact",Schema ="hr")]
    public partial class EmployeeEmergencyContact : GenericEntity<string>
    {
        public string address { get; set; }
        public string employee_basic_info_id { get; set; }
        public string full_name { get; set; }
        public string other_no { get; set; }
        public string phone_no { get; set; }
        public string family_status_id { get; set; }
    }
}