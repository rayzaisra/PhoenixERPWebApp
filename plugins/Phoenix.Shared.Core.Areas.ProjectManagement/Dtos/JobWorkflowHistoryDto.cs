using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobWorkflowHistoryDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string current_status_created_by { get; set; }
        public DateTime? current_status_created_date { get; set; }
        [StringLength(50)]
        public string current_status_id { get; set; }
        [StringLength(50)]
        public string job_number { get; set; }
        [StringLength(50)]
        public string last_status_created_by { get; set; }
        public DateTime? last_status_created_date { get; set; }
        [StringLength(50)]
        public string last_status_id { get; set; }
        public int? seq { get; set; }
    }
    public class JobWorkflowHistoryNew
    {
        [StringLength(50)]
        public string current_status_created_by { get; set; }
        public DateTime? current_status_created_date { get; set; }
        [StringLength(50)]
        public string current_status_id { get; set; }
        [StringLength(50)]
        public string job_number { get; set; }
        [StringLength(50)]
        public string last_status_created_by { get; set; }
        public DateTime? last_status_created_date { get; set; }
        [StringLength(50)]
        public string last_status_id { get; set; }
        public int? seq { get; set; }
    }            
    public class JobWorkflowHistorySearch : SearchParameter
    {
        public string Search_current_status_created_by { get; set; }
        public DateTime? Search_current_status_created_date { get; set; }
        public string Search_current_status_id { get; set; }
        public string Search_job_number { get; set; }
        public string Search_last_status_created_by { get; set; }
        public DateTime? Search_last_status_created_date { get; set; }
        public string Search_last_status_id { get; set; }
        public int? Search_seq { get; set; }
        
    }
}