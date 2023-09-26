using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class OvertimeDto
    {
        [Required]
            public string Id { get; set; }
        public int? amount { get; set; }
        [StringLength(50)]
        public string approval { get; set; }
        [StringLength(50)]
        public string approval_by { get; set; }
        public DateTime? approval_date { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        [Required]
        public string hour_overtime { get; set; }
        public DateTime? last_update { get; set; }
        [Required]
        public DateTime? overtime_date { get; set; }
    }
    public class OvertimeNew
    {
        public int? amount { get; set; }
        [StringLength(50)]
        public string approval { get; set; }
        [StringLength(50)]
        public string approval_by { get; set; }
        public DateTime? approval_date { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        [Required]
        public string hour_overtime { get; set; }
        public DateTime? last_update { get; set; }
        [Required]
        public DateTime? overtime_date { get; set; }
    }            
    public class OvertimeSearch : SearchParameter
    {
        public int? Search_amount { get; set; }
        public string Search_approval { get; set; }
        public string Search_approval_by { get; set; }
        public DateTime? Search_approval_date { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_hour_overtime { get; set; }
        public DateTime? Search_last_update { get; set; }
        public DateTime? Search_overtime_date { get; set; }
        
    }
}