using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TmUserApprovalDto
    {
        [Required]
            public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsCondition { get; set; }
        public string TmTempApprovalId { get; set; }
        public int? IndexUser { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string job_title_id { get; set; }
        public string job_title_name { get; set; }
        public string employee_full_name { get; set; }

    }
    public class TmUserApprovalNew
    {
		public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsCondition { get; set; }
        public string TmTempApprovalId { get; set; }
        public int? IndexUser { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }            
    public class SearchTmUserApproval : SearchParameter
    {
        
        
    }
}