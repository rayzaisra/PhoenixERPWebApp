using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class SalaryProcessComponentDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string component_name { get; set; }
        public long? employee_basic_info_id { get; set; }
        public int? month { get; set; }
        public long? tax_id { get; set; }
        [StringLength(100)]
        public string type { get; set; }
        public long? value { get; set; }
        public long? value_year { get; set; }
        public int? year { get; set; }
    }
    public class SalaryProcessComponentNew
    {
        [StringLength(100)]
        public string component_name { get; set; }
        public long? employee_basic_info_id { get; set; }
        public int? month { get; set; }
        public long? tax_id { get; set; }
        [StringLength(100)]
        public string type { get; set; }
        public long? value { get; set; }
        public long? value_year { get; set; }
        public int? year { get; set; }
    }            
    public class SalaryProcessComponentSearch : SearchParameter
    {
        public string Search_component_name { get; set; }
        public long? Search_employee_basic_info_id { get; set; }
        public int? Search_month { get; set; }
        public long? Search_tax_id { get; set; }
        public string Search_type { get; set; }
        public long? Search_value { get; set; }
        public long? Search_value_year { get; set; }
        public int? Search_year { get; set; }
        
    }
}