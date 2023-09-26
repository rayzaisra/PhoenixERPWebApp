using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientBriefAccountDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string account_management_id { get; set; }
        [StringLength(50)]
        public string approval_status_id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
    }
    public class ClientBriefAccountNew
    {
        [StringLength(50)]
        public string account_management_id { get; set; }
        [StringLength(50)]
        public string approval_status_id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
    }            
    public class ClientBriefAccountSearch : SearchParameter
    {
        public string Search_account_management_id { get; set; }
        public string Search_approval_status_id { get; set; }
        public string Search_client_brief_id { get; set; }
        public string Search_job_posting_requisition_id { get; set; }
        
    }
}