using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EducationDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(100)]
        //[Required]
        public string address { get; set; }
        [StringLength(50)]
        [Required]
        public string education_level_id { get; set; }
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        [Required]
        public string end_education { get; set; }
        [StringLength(60)]
        [Required]
        public string institution_id { get; set; }
        [StringLength(100)]
        public string majors { get; set; }
        [StringLength(100)]
        [Required]
        public string start_education { get; set; }
        [StringLength(200)]
        public string institution_name { get; set; }
        [StringLength(200)]
        public string qualification { get; set; }
        [StringLength(200)]
        public string education_level_name { get; set; }
        public string education_institusion_id_other { get; set; }
    }
    public class EducationNew
    {
        public string Id { get; set; }
        [StringLength(100)]
        //[Required]
        public string address { get; set; }
        [StringLength(50)]
        [Required]
        public string education_level_id { get; set; }
       
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        [Required]
        public string end_education { get; set; }
        [StringLength(60)]
        //[Required]
        public string institution_id { get; set; }
        [StringLength(100)]
        public string majors { get; set; }
        [StringLength(100)]
        [Required]
        public string start_education { get; set; }
        [StringLength(200)]
        public string qualification { get; set; }
        [StringLength(200)]
        public string education_level_name { get; set; }
        public string institution_name { get; set; }
        public string education_institusion_id_other { get; set; }
    }
    public class EducationSearch : SearchParameter
    {
        public string Search_address { get; set; }
        public string Search_education_level_id { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_end_education { get; set; }
        public string Search_institution_id { get; set; }
        public string Search_majors { get; set; }
        public string Search_start_education { get; set; }

    }
}