using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobInterviewDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string from_email { get; set; }
        [StringLength(50)]
        public string interviewer { get; set; }
        public DateTime? interview_date { get; set; }
        public DateTime? interview_time { get; set; }
        [StringLength(2147483647)]
        public string job_interview_description { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string latitude { get; set; }
        [StringLength(50)]
        public string longitude { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(200)]
        public string place_of_interview { get; set; }
        [StringLength(50)]
        public string to_email { get; set; }
    }
    public class JobInterviewNew
    {
        [StringLength(50)]
        public string from_email { get; set; }
        [StringLength(50)]
        public string interviewer { get; set; }
        public DateTime? interview_date { get; set; }
        public DateTime? interview_time { get; set; }
        [StringLength(2147483647)]
        public string job_interview_description { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string latitude { get; set; }
        [StringLength(50)]
        public string longitude { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(200)]
        public string place_of_interview { get; set; }
        [StringLength(50)]
        public string to_email { get; set; }
    }            
    public class JobInterviewSearch : SearchParameter
    {
        public string Search_from_email { get; set; }
        public string Search_interviewer { get; set; }
        public DateTime? Search_interview_date { get; set; }
        public DateTime? Search_interview_time { get; set; }
        public string Search_job_interview_description { get; set; }
        public string Search_job_posting_requisition_id { get; set; }
        public string Search_latitude { get; set; }
        public string Search_longitude { get; set; }
        public string Search_notes { get; set; }
        public string Search_place_of_interview { get; set; }
        public string Search_to_email { get; set; }
        
    }
}