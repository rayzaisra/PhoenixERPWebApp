using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class BasicInfoGradeTittleDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        public string job_grade_id { get; set; }
        [StringLength(50)]
        public string job_tittle_id { get; set; }
    }
    public class BasicInfoGradeTittleNew
    {
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        public string job_grade_id { get; set; }
        [StringLength(50)]
        public string job_tittle_id { get; set; }
    }            
    public class BasicInfoGradeTittleSearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public string Search_job_grade_id { get; set; }
        public string Search_job_tittle_id { get; set; }
        
    }
}