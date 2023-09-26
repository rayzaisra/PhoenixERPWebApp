using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeAbsenceDto
    {
        [Required]
            public string Id { get; set; }
        [Required]
        public DateTime? date_absence { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        public string note { get; set; }
        [Required]
        public DateTime? time_back { get; set; }
        [Required]
        public DateTime? time_come { get; set; }
    }
    public class EmployeeAbsenceNew
    {
        [Required]
        public DateTime? date_absence { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        public string note { get; set; }
        [Required]
        public DateTime? time_back { get; set; }
        [Required]
        public DateTime? time_come { get; set; }
    }            
    public class EmployeeAbsenceSearch : SearchParameter
    {
        public DateTime? Search_date_absence { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_note { get; set; }
        public DateTime? Search_time_back { get; set; }
        public DateTime? Search_time_come { get; set; }
        
    }
}