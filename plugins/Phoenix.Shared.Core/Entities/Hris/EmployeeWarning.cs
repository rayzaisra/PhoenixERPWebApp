using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_warning",Schema ="hr")]
    public partial class EmployeeWarning : GenericEntity<string>
    {
        public string description { get; set; }
        public string notes { get; set; }
        public string subject { get; set; }
        public string warning_by { get; set; }
        public DateTime? warning_date { get; set; }
        public string warning_to { get; set; }
    }
}