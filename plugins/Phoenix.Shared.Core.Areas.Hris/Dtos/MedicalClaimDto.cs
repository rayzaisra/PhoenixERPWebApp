using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MedicalClaimDto
    {
        [Required]
            public string Id { get; set; }
        [Required]
        public DateTime? date_claim { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [Required]
        public int? value { get; set; }
        [StringLength(50)]
        [Required]
        public string year_periode { get; set; }
    }
    public class MedicalClaimNew
    {
        [Required]
        public DateTime? date_claim { get; set; }
        [StringLength(50)]
        [Required]
        public string employee_basic_info_id { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        [Required]
        public int? value { get; set; }
        [StringLength(50)]
        [Required]
        public string year_periode { get; set; }
    }            
    public class MedicalClaimSearch : SearchParameter
    {
        public DateTime? Search_date_claim { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_reason { get; set; }
        public int? Search_value { get; set; }
        public string Search_year_periode { get; set; }
        
    }
}