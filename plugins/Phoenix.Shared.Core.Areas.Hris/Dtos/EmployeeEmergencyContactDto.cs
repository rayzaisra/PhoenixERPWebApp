using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeEmergencyContactDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string address { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        public string full_name { get; set; }
        [StringLength(50)]
        public string other_no { get; set; }
        [StringLength(50)]
        public string phone_no { get; set; }
        [StringLength(50)]
        public string family_status_id { get; set; }
        [StringLength(50)]
        public string relation_name { get; set; }
    }
    public class EmployeeEmergencyContactNew
    {
        public string Id { get; set; }
        [StringLength(100)]
        public string address { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(100)]
        public string full_name { get; set; }
        [StringLength(50)]
        public string other_no { get; set; }
        [StringLength(50)]
        public string phone_no { get; set; }
        [StringLength(50)]
        public string family_status_id { get; set; }
        [StringLength(50)]
        public string relation_name { get; set; }
    }            
    public class EmployeeEmergencyContactSearch : SearchParameter
    {
        public string Search_address { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_full_name { get; set; }
        public string Search_other_no { get; set; }
        public string Search_phone_no { get; set; }
        public string Search_relation { get; set; }
        
    }
}