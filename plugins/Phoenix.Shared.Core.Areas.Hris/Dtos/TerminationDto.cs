using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TerminationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [Required]
        public DateTime? termination_date { get; set; }
    }
    public class TerminationNew
    {
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [Required]
        public DateTime? termination_date { get; set; }
    }            
    public class TerminationSearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public string Search_reason { get; set; }
        public DateTime? Search_termination_date { get; set; }
        
    }
}