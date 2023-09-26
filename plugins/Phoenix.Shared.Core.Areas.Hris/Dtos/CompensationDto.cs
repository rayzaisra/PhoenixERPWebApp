using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CompensationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string compensation_id { get; set; }
        [Required]
        public DateTime? date_compensation { get; set; }
        [Required]
        public long? employee_basic_info_id { get; set; }
        public DateTime? end_compensation { get; set; }
        [StringLength(50)]
        public string type { get; set; }
        [Required]
        public int? value { get; set; }
    }
    public class CompensationNew
    {
        [StringLength(50)]
        [Required]
        public string compensation_id { get; set; }
        [Required]
        public DateTime? date_compensation { get; set; }
        [Required]
        public long? employee_basic_info_id { get; set; }
        public DateTime? end_compensation { get; set; }
        [StringLength(50)]
        public string type { get; set; }
        [Required]
        public int? value { get; set; }
    }            
    public class CompensationSearch : SearchParameter
    {
        public string Search_compensation_id { get; set; }
        public DateTime? Search_date_compensation { get; set; }
        public long? Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_compensation { get; set; }
        public string Search_type { get; set; }
        public int? Search_value { get; set; }
        
    }
}