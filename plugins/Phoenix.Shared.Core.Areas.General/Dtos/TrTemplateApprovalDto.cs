using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TrTemplateApprovalDto
    {
        [Required]
            public string Id { get; set; }
        public string RefId { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int? DueDate { get; set; }
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public int? StatusApproved { get; set; }
        public string RemarkRejected { get; set; }
        public string DetailLink { get; set; }
        public string FormReqName { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public List<TrUserApprovalDto> listUserApproval { get; set; }
    }
    public class TrTemplateApprovalNew
    {
        public string Id { get; set; }
        public string RefId { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int? DueDate { get; set; }
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public int? StatusApproved { get; set; }
        public string RemarkRejected { get; set; }
        public string DetailLink { get; set; }
        public string FormReqName { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }            
    public class SearchTrTemplateApproval : SearchParameter
    {
        public string Search_RefId { get; set; }
        

    }
}