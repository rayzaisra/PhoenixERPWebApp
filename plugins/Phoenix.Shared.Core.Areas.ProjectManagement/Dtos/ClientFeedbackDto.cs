using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ClientFeedbackDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string approval_type_id { get; set; }
        [StringLength(50)]
        public string client_feedback_file_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
    }
    public class ClientFeedbackNew
    {
        [StringLength(50)]
        public string approval_type_id { get; set; }
        [StringLength(50)]
        public string client_feedback_file_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
    }
    public class ClientFeedbackSearch : SearchParameter
    {
        public string Search_approval_type_id { get; set; }
        public string Search_client_feedback_file_id { get; set; }
        public string Search_job_posting_requisition_id { get; set; }

    }

    public class ClientFeedbackDetailDto
    {
        public string Id { get; set; }
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string CampaignName { get; set; }
        public string ApprovalStatusId { get; set; }
        public string ClientFeedbackFileId { get; set; }
        
    }

    public class ClientFeedbackParameterDto
    {

        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string ApprovalTypeId { get; set; }
        public string FileNameId { get; set; }
    }

    public class ClientFeedbackResponseDto
    {
        public string Id { get; set; }
        public string JobName { get; set; }
        public string JobNumber { get; set; }
        public string FileName { get; set; }
        public string ApprovalStatusId { get; set; }
        public bool UpdateDataResult { get; set; }
    }
}