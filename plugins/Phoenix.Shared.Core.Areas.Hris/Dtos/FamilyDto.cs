using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class FamilyDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string address { get; set; }
        public string birthday { get; set; }
        [StringLength(2147483647)]
        public string description { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        [Required]
        public string family_status_id { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(50)]
        public string institution_id { get; set; }
        [StringLength(50)]
        public string married_status_id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [StringLength(50)]
        public string other_no { get; set; }
        [StringLength(50)]
        public string phone_no { get; set; }
        [StringLength(50)]
        public string place_birth { get; set; }
        [StringLength(50)]
        public string religion_id { get; set; }
        [StringLength(50)]
        public string family_status_name { get; set; }
        public string education_level_id { get; set; }
        public string family_education_institusion_id_other { get; set; }
    }
    public class FamilyNew
    {
        public string Id { get; set; }
        [StringLength(100)]
        public string address { get; set; }
        public string birthday { get; set; }
        [StringLength(2147483647)]
        public string description { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        [Required]
        public string family_status_id { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(50)]
        public string institution_id { get; set; }
        [StringLength(50)]
        public string married_status_id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [StringLength(50)]
        public string other_no { get; set; }
        [StringLength(50)]
        public string phone_no { get; set; }
        [StringLength(50)]
        public string place_birth { get; set; }
        [StringLength(50)]
        public string religion_id { get; set; }
        public string education_level_id { get; set; }
        public string family_status_name { get; set; }
        public string family_education_institusion_id_other { get; set; }
    }            
    public class FamilySearch : SearchParameter
    {
        public string Search_address { get; set; }
        public DateTime? Search_birthday { get; set; }
        public string Search_description { get; set; }
        public string Search_education_id { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_family_status_id { get; set; }
        public string Search_gender { get; set; }
        public string Search_institution_id { get; set; }
        public string Search_married_status_id { get; set; }
        public string Search_name { get; set; }
        public string Search_other_no { get; set; }
        public string Search_phone_no { get; set; }
        public string Search_place_birth { get; set; }
        public string Search_religion_id { get; set; }
        
    }
}