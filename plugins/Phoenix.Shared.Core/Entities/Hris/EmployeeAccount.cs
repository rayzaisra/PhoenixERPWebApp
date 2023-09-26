using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_account",Schema ="hr")]
    public partial class EmployeeAccount : GenericEntity<string>
    {
        public string account_no { get; set; }
        public string bankbranch_id { get; set; }
        public string employeeaccount_id { get; set; }
        public string employee_basic_info_id { get; set; }
    }
}