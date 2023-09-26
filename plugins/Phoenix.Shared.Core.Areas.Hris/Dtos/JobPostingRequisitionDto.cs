using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobPostingRequisitionDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        public int? candidate_age_range_end { get; set; }
        public int? candidate_age_range_start { get; set; }
        [StringLength(2147483647)]
        public string candidate_experience { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string candidate_qualification { get; set; }
        [StringLength(50)]
        [Required]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        [StringLength(2147483647)]
        public string job_post_description { get; set; }
        [StringLength(50)]
        [Required]
        public string job_title { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public int? number_of_position { get; set; }
        [StringLength(50)]
        public string reg_code { get; set; }
        public int? salary_end { get; set; }
        public int? salary_start { get; set; }
        [StringLength(50)]
        public string status { get; set; }
    }
    public class JobPostingRequisitionNew
    {
        [StringLength(50)]
        public string business_unit_id { get; set; }
        public int? candidate_age_range_end { get; set; }
        public int? candidate_age_range_start { get; set; }
        [StringLength(2147483647)]
        public string candidate_experience { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string candidate_qualification { get; set; }
        [StringLength(50)]
        [Required]
        public string central_resource_id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string code { get; set; }
        [StringLength(2147483647)]
        public string job_post_description { get; set; }
        [StringLength(50)]
        [Required]
        public string job_title { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public int? number_of_position { get; set; }
        [StringLength(50)]
        public string reg_code { get; set; }
        public int? salary_end { get; set; }
        public int? salary_start { get; set; }
        [StringLength(50)]
        public string status { get; set; }
    }            
    public class JobPostingRequisitionSearch : SearchParameter
    {
        public string Search_business_unit_id { get; set; }
        public int? Search_candidate_age_range_end { get; set; }
        public int? Search_candidate_age_range_start { get; set; }
        public string Search_candidate_experience { get; set; }
        public string Search_candidate_qualification { get; set; }
        public string Search_central_resource_id { get; set; }
        public string Search_client_brief_id { get; set; }
        public string Search_code { get; set; }
        public string Search_job_post_description { get; set; }
        public string Search_job_title { get; set; }
        public string Search_notes { get; set; }
        public int? Search_number_of_position { get; set; }
        public string Search_reg_code { get; set; }
        public int? Search_salary_end { get; set; }
        public int? Search_salary_start { get; set; }
        public string Search_status { get; set; }
        
    }
}