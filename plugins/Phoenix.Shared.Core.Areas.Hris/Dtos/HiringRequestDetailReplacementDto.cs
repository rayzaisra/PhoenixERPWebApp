using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class HiringRequestDetailReplacementDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(255)]
        public string employment_status { get; set; }
        public double? mpp_budged_id { get; set; }
        [StringLength(255)]
        public string ttf { get; set; }
    }
    public class HiringRequestDetailReplacementNew
    {
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(255)]
        public string employment_status { get; set; }
        public double? mpp_budged_id { get; set; }
        [StringLength(255)]
        public string ttf { get; set; }
    }            
    public class HiringRequestDetailReplacementSearch : SearchParameter
    {
        public string Search_employee_basic_info_id { get; set; }
        public string Search_employment_status { get; set; }
        public double? Search_mpp_budged_id { get; set; }
        public string Search_ttf { get; set; }
        
    }
}