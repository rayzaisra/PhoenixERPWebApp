using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string account_no { get; set; }
        [StringLength(50)]
        [Required]
        public string bankbranch_id { get; set; }
        [StringLength(50)]
        [Required]
        public string Employee_id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
    }
    public class EmployeeNew
    {
        [StringLength(50)]
        [Required]
        public string account_no { get; set; }
        [StringLength(50)]
        [Required]
        public string bankbranch_id { get; set; }
        [StringLength(50)]
        [Required]
        public string Employee_id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
    }            
    public class EmployeeSearch : SearchParameter
    {
        public string Search_account_no { get; set; }
        public string Search_bankbranch_id { get; set; }
        public string Search_Employee_id { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        
    }
}