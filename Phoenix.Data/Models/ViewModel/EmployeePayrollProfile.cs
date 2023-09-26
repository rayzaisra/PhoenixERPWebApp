using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    public class EmployeePayrollProfile
    {
        public string EmployeeId { get; set; }
        public string PtkpSettingId { get; set; }
        public List<EmployeeSubcomponentAdditional> EmployeeSubcomponentAdditionals { get; set; }
        public List<EmployeeSubcomponentDeduction> EmployeeSubcomponentDeductions { get; set; }
    }
}
