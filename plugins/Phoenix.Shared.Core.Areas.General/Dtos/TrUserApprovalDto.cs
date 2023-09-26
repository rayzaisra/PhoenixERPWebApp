using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TrUserApprovalDto
    {
        [Required]
            public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsCondition { get; set; }
        public string TrTempApprovalId { get; set; }
        public int? IndexUser { get; set; }
        public int? StatusApproved { get; set; }
        public int? Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string job_title_id { get; set; }
        public string job_title_name { get; set; }
        public string employee_full_name { get; set; }
        public string approved_by { get; set; }
        public DateTime? created_on { get; set; }
        public bool? IsSpecialCase { get; set; }
        public string RemarkSpecialCase { get; set; }
    }
    public class TrUserApprovalNew
    {
		public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsCondition { get; set; }
        public string TrTempApprovalId { get; set; }
        public int? IndexUser { get; set; }
        public int? StatusApproved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsSpecialCase { get; set; }
        public string RemarkSpecialCase { get; set; }
    }
            
    public class SearchTrUserApproval : SearchParameter
    {
        
        
    }
}