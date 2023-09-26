using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeExperienceDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string address { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        [Required]
        public string end_date { get; set; }
        [StringLength(50)]
        [Required]
        public string company_name { get; set; }
        [StringLength(50)]
        [Required]
        public string phone { get; set; }
        [StringLength(50)]
        [Required]
        public string position { get; set; }
        [StringLength(100)]
        [Required]
        public string start_date { get; set; }
        [StringLength(100)]
        public string remark { get; set; }
    }
    public class EmployeeExperienceNew
    {
        public string Id { get; set; }
        [StringLength(50)]
        //[Required]
        public string address { get; set; }
        [StringLength(50)]
      
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        //[Required]
        public string end_date { get; set; }
        [StringLength(50)]
        //[Required]
        public string company_name { get; set; }
        [StringLength(50)]
        //[Required]
        public string phone { get; set; }
        [StringLength(50)]
        //[Required]
        public string position { get; set; }
        [StringLength(100)]
        //[Required]
        public string start_date { get; set; }
        [StringLength(100)]
        public string remark { get; set; }
    }            
    public class EmployeeExperienceSearch : SearchParameter
    {
        public string Search_address { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_end_date { get; set; }
        public string Search_company_name { get; set; }
        public string Search_phone { get; set; }
        public string Search_position { get; set; }
        public string Search_start_date { get; set; }
        
    }
}