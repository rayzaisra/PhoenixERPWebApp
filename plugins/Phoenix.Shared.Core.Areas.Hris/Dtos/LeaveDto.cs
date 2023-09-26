using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class LeaveDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
        [StringLength(60)]
        [Required]
        public string status { get; set; }
    }
    public class LeaveNew
    {
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
        [StringLength(60)]
        [Required]
        public string status { get; set; }
    }            
    public class LeaveSearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_date { get; set; }
        public string Search_reason { get; set; }
        public DateTime? Search_start_date { get; set; }
        public string Search_status { get; set; }
        
    }
}