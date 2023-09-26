using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TmTemplateApprovalDto
    {
        [Required]
            public string Id { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int? DueDate { get; set; }
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string JobTitleId { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string BusinessUnitName { get; set; }
        public List<TmUserApprovalDto> listTmUserApproval { get; set; } 
        public TransApprovalVm dataVm { get; set; }
		public string ForDivisionId { get; set; }
        public bool? IsDivisionAll { get; set; }
        public bool? IsSpecialCase { get; set; }
    }

    public class TransApprovalVm
    {
        [Required]
        public string MenuId { get; set; }
        [Required]
        public string RefId { get; set; }
        [Required]
        public string DetailLink { get; set; }
        [Required]
        public string Tname { get; set; }
        public string IdTemplate { get; set; }
    }
    public class TmTemplateApprovalNew
    {
		public string Id { get; set; }
        public string TemplateName { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int? DueDate { get; set; }
        public int? Reminder { get; set; }
        public int? ApprovalCategory { get; set; }
        public int? ApprovalType { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string JobTitleId { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string MenuId { get; set; }
    }            
    public class SearchTmTemplateApproval : SearchParameter
    {
        
        public string Search_MenuName { get; set; }
        public string Search_TemplateName { get; set; }
        public string Search_BusinessUnitName { get; set; }
        public string Search_BusinessUnitId { get; set; }
        public string Search_Budgeted { get; set; }
        public string Search_Unbudgeted { get; set; }
        public string Search_Replacement { get; set; }
    }
}