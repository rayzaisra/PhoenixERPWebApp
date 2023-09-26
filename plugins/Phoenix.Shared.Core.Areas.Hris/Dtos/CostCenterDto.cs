using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CostCenterDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string activity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string cost_id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [Required]
        public DateTime? end_periode { get; set; }
        [StringLength(50)]
        [Required]
        public string percentage { get; set; }
        [Required]
        public DateTime? periode { get; set; }
    }
    public class CostCenterNew
    {
        [StringLength(50)]
        [Required]
        public string activity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string cost_id { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [Required]
        public DateTime? end_periode { get; set; }
        [StringLength(50)]
        [Required]
        public string percentage { get; set; }
        [Required]
        public DateTime? periode { get; set; }
    }            
    public class CostCenterSearch : SearchParameter
    {
        public string Search_activity_id { get; set; }
        public string Search_cost_id { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_periode { get; set; }
        public string Search_percentage { get; set; }
        public DateTime? Search_periode { get; set; }
        
    }
}