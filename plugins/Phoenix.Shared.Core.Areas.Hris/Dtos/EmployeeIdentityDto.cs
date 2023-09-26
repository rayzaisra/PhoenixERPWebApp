using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeIdentityDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? expired { get; set; }
        [StringLength(150)]
        public string file_upload { get; set; }
        [StringLength(50)]
        public string name_card { get; set; }
        [StringLength(50)]
        public string no_card { get; set; }
        public DateTime? published { get; set; }
        [StringLength(200)]
        public string published_by { get; set; }
    }
    public class EmployeeIdentityNew
    {
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? expired { get; set; }
        [StringLength(150)]
        public string file_upload { get; set; }
        [StringLength(50)]
        public string name_card { get; set; }
        [StringLength(50)]
        public string no_card { get; set; }
        public DateTime? published { get; set; }
        [StringLength(200)]
        public string published_by { get; set; }
    }            
    public class EmployeeIdentitySearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_expired { get; set; }
        public string Search_file_upload { get; set; }
        public string Search_name_card { get; set; }
        public string Search_no_card { get; set; }
        public DateTime? Search_published { get; set; }
        public string Search_published_by { get; set; }
        
    }
}