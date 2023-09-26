using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class ResignationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? notice_date { get; set; }
        public DateTime? resignation_date { get; set; }
        [StringLength(2147483647)]
        public string resignation_reason { get; set; }
    }
    public class ResignationNew
    {
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? notice_date { get; set; }
        public DateTime? resignation_date { get; set; }
        [StringLength(2147483647)]
        public string resignation_reason { get; set; }
    }            
    public class ResignationSearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public string Search_notes { get; set; }
        public DateTime? Search_notice_date { get; set; }
        public DateTime? Search_resignation_date { get; set; }
        public string Search_resignation_reason { get; set; }
        
    }
}