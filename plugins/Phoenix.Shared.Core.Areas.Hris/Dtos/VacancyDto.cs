using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class VacancyDto
    {
        [Required]
            public string Id { get; set; }
        public int? applicant_total { get; set; }
        [StringLength(50)]
        public string approval_id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        public int? hiring_request_id { get; set; }
        [StringLength(50)]
        public string reg { get; set; }
        [StringLength(255)]
        public string vacancy_name { get; set; }
    }
    public class VacancyNew
    {
        public int? applicant_total { get; set; }
        [StringLength(50)]
        public string approval_id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        public int? hiring_request_id { get; set; }
        [StringLength(50)]
        public string reg { get; set; }
        [StringLength(255)]
        public string vacancy_name { get; set; }
    }            
    public class VacancySearch : SearchParameter
    {
        public int? Search_applicant_total { get; set; }
        public string Search_approval_id { get; set; }
        public string Search_business_unit_id { get; set; }
        public int? Search_hiring_request_id { get; set; }
        public string Search_reg { get; set; }
        public string Search_vacancy_name { get; set; }
        
    }
}