using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeOutDto
    {
        [Required]
            public string Id { get; set; }
        [Required]
        public DateTime? date_out { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [StringLength(50)]
        [Required]
        public string time_comeback { get; set; }
        [StringLength(50)]
        [Required]
        public string time_out { get; set; }
        [StringLength(50)]
        public string type { get; set; }
    }
    public class EmployeeOutNew
    {
        [Required]
        public DateTime? date_out { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [StringLength(50)]
        [Required]
        public string time_comeback { get; set; }
        [StringLength(50)]
        [Required]
        public string time_out { get; set; }
        [StringLength(50)]
        public string type { get; set; }
    }            
    public class EmployeeOutSearch : SearchParameter
    {
        public DateTime? Search_date_out { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_reason { get; set; }
        public string Search_time_comeback { get; set; }
        public string Search_time_out { get; set; }
        public string Search_type { get; set; }
        
    }
}