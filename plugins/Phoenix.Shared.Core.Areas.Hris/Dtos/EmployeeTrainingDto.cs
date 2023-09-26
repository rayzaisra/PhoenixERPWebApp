using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeTrainingDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string address { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        [Required]
        public string end_training { get; set; }
        public string expired_certificate { get; set; }
        [StringLength(150)]
        public string filemaster_id { get; set; }
        [StringLength(50)]
        [Required]
        public string institution_id { get; set; }
        [StringLength(50)]
        [Required]
        public string name_training { get; set; }
        [StringLength(50)]
        [Required]
        public string no_certification { get; set; }
        [StringLength(100)]
        [Required]
        public string start_training { get; set; }
        [StringLength(50)]
        public string institution_name { get; set; }
        public string course_institusion_id_other { get; set; }
    }
    public class EmployeeTrainingNew
    {
        public string Id { get; set; }
        [StringLength(50)]
        //[Required]
        public string address { get; set; }
        [StringLength(50)]
       
        public string description { get; set; }
        [StringLength(50)]
       
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        [Required]
        public string end_training { get; set; }
        public string expired_certificate { get; set; }
        [StringLength(150)]
        public string filemaster_id { get; set; }
        [StringLength(50)]
        //[Required]
        public string institution_id { get; set; }
        [StringLength(50)]
        public string institution_name { get; set; }
        [StringLength(50)]
        [Required]
        public string name_training { get; set; }
        [StringLength(50)]
        //[Required]
        public string no_certification { get; set; }
        [StringLength(100)]
        [Required]
        public string start_training { get; set; }
        public string course_institusion_id_other { get; set; }
    }            
    public class EmployeeTrainingSearch : SearchParameter
    {
        public string Search_address { get; set; }
        public string Search_description { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_end_training { get; set; }
        public DateTime? Search_expired_certificate { get; set; }
        public string Search_file_upload { get; set; }
        public string Search_institusion_id { get; set; }
        public string Search_name_training { get; set; }
        public string Search_no_certification { get; set; }
        public string Search_start_training { get; set; }
        
    }
}