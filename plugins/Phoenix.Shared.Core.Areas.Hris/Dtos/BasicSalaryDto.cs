using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class BasicSalaryDto
    {
        [Required]
            public Guid Id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
        [Required]
        public int? value { get; set; }
    }
    public class BasicSalaryNew
    {
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
        [Required]
        public int? value { get; set; }
    }            
    public class BasicSalarySearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_date { get; set; }
        public DateTime? Search_start_date { get; set; }
        public int? Search_value { get; set; }
        
    }
}