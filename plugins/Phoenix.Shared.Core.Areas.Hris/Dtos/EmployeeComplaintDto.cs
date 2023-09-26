using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeComplaintDto
    {
        [Required]
            public string Id { get; set; }
        public DateTime? complaint_date { get; set; }
        [StringLength(2147483647)]
        public string complaint_description { get; set; }
        [StringLength(150)]
        public string complaint_title { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
    }
    public class EmployeeComplaintNew
    {
        public DateTime? complaint_date { get; set; }
        [StringLength(2147483647)]
        public string complaint_description { get; set; }
        [StringLength(150)]
        public string complaint_title { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
    }            
    public class EmployeeComplaintSearch : SearchParameter
    {
        public DateTime? Search_complaint_date { get; set; }
        public string Search_complaint_description { get; set; }
        public string Search_complaint_title { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_notes { get; set; }
        
    }
}