using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_complaint",Schema ="hr")]
    public partial class EmployeeComplaint : GenericEntity<string>
    {
        public DateTime? complaint_date { get; set; }
        public string complaint_description { get; set; }
        public string complaint_title { get; set; }
        public string employee_basic_info_id { get; set; }
        public string notes { get; set; }
    }
}