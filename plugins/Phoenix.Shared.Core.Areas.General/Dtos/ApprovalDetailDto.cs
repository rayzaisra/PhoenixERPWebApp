using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApprovalDetailDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string approval_id { get; set; }
        public int? approval_level { get; set; }
        [StringLength(50)]
        [Required]
        public string approval_status_id { get; set; }
        [StringLength(50)]
        [Required]
        public string approver_id { get; set; }
        public DateTime? expiry_datetime { get; set; }
        public bool is_auto_approved { get; set; }
        [StringLength(50)]
        public string previous_approval { get; set; }
    }
    public class ApprovalDetailNew
    {
        [StringLength(50)]
        [Required]
        public string approval_id { get; set; }
        public int? approval_level { get; set; }
        [StringLength(50)]
        [Required]
        public string approval_status_id { get; set; }
        [StringLength(50)]
        [Required]
        public string approver_id { get; set; }
        public DateTime? expiry_datetime { get; set; }
        public bool is_auto_approved { get; set; }
        [StringLength(50)]
        public string previous_approval { get; set; }
    }            
    public class ApprovalDetailSearch : SearchParameter
    {
        public string Search_approval_id { get; set; }
        public int? Search_approval_level { get; set; }
        public string Search_approval_status_id { get; set; }
        public string Search_approver_id { get; set; }
        public DateTime? Search_expiry_datetime { get; set; }
        public bool Search_is_auto_approved { get; set; }
        public string Search_previous_approval { get; set; }
        
    }
}