using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    public class GroupViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string GroupName { get; set; }
    }
    public class MenuViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string MenuName { get; set; }
        [Required]
        public string MenuLink { get; set; }
        [Required]
        public string MenuUnique { get; set; }
        [Required]
        public string ParentId { get; set; }
    }


    public class GroupAccessViewModel
    {
        [Required]
        public string GroupId { get; set; }
        [Required]
        public string MenuId { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
    }

    public class UserAppViewModel
    {
        public string Id { get; set; }
        public string AliasName { get; set; }
        public string GroupId { get; set; }
        public string Email { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsMsUser { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsUserNeedTimesheet { get; set; }
    }

    public class ChangePwordVm
    {
        [Required]
        [MaxLength(20)]
        public string newpwd { get; set; }

        [Required]
        [MaxLength(20)]
        public string confirmpwd { get; set; }
    }

    public class EmployeeMappingDivisionViewModel
    {
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BusinessUnitId { get; set; }
        public string DivisionId { get; set; }
        public string DivisiName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitleName { get; set; }
        public string NickName { get; set; }
        public bool IsReadyInUserApps { get; set; }
    }
    public class MappingGroupUserViewModel
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public List<UserAppMappingGroupViewModel> ListUsers { get; set; }
    }
    public class UserAppMappingGroupViewModel
    {
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BusinessUnitId { get; set; }
        public string DivisionId { get; set; }
        public string DivisiName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitleName { get; set; }
        public string NickName { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }


    public class UserAccessViewModel
    {
        [Required]
        public string GroupId { get; set; }
        [Required]
        public string MenuId { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
    }

    public class TempApprovalViewModel
    {
        public string Id { get; set; }
        [Required]
        public string TemplateName { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int DueDate { get; set; }
        public int Reminder { get; set; }
        [Required]
        public int ApprovalCategory { get; set; }
        [Required]
        public int ApprovalType { get; set; }

        [Required]
        public List<TempUserApprovalViewModel> ListUserApprovals { get; set; }

        ///Additional...
        ///        
        public string JobTitleId { get; set; }
        public string MenuId { get; set; }
        //public string BussinessUnitDivisionId { get; set; }
        //public string BussinessUnitJobLevelId { get; set; }
        public string[] ForDivisionId { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public bool IsAllDivision { get; set; }
        public bool IsSpecialCase { get; set; }
    }

    public class TrApprovalViewModel
    {
        public string Id { get; set; }
        [Required]
        public string RefId { get; set; }
        [Required]
        public string TemplateName { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int DueDate { get; set; }
        [Required]
        public string DetailLink { get; set; }
        public int Reminder { get; set; }
        [Required]
        public int ApprovalCategory { get; set; }
        [Required]
        public int ApprovalType { get; set; }
        [Required]
        public string FormReqName { get; set; }

        [Required]
        public string Tname { get; set; }

        [Required]
        public List<TempUserApprovalViewModel> ListUserApprovals { get; set; }
    }
    public class TransApprovalHrisVm
    {
        [Required]
        public string MenuId { get; set; }
        [Required]
        public string RefId { get; set; }
        [Required]
        public string DetailLink { get; set; }

        public string NotificationType { get; set; }
        public string Module { get; set; }

        /// <summary>
        /// Table Name
        /// </summary>
        [Required]
        public string Tname { get; set; }
        public string IdTemplate { get; set; }
        public string Id { get; set; }

    }
    public class TransApprovalFnVm
    {
        [Required]
        public string MenuId { get; set; }
        [Required]
        public string RefId { get; set; }
        [Required]
        public string DetailLink { get; set; }

        /// <summary>
        /// Table Name
        /// </summary>
        [Required]
        public string Tname { get; set; }
        [Required]
        public decimal? Budget { get; set; }
    }

    public class TempUserApprovalViewModel
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public int IndexUser { get; set; }
        public bool? IsCondition { get; set; }
        public string TmTempApprovalId { get; set; }
        public bool? IsSpecialCase { get; set; }

        public List<TempConditionalApprovalViewModel> ListConditionalApprovals { get; set; }
    }

    public class TempConditionalApprovalViewModel
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public int GroupConditionIndex { get; set; }
        public string GroupConditionApproval { get; set; }
        public string ConditionalApproval { get; set; }
        public string TemplateUserApprovalId { get; set; }
    }


    public class TemplateApprovalVm
    {
        public string Id { get; set; }
        [Required]
        public string TemplateName { get; set; }
        public bool? IsNotifyByEmail { get; set; }
        public bool? IsNotifyByWeb { get; set; }
        public int DueDate { get; set; }
        public int Reminder { get; set; }
        public int ApprovalCategory { get; set; }
        public int ApprovalType { get; set; }
        public List<TemplateUserApprovalVm> ListUserApprovals { get; set; }
    }

    public class TemplateUserApprovalVm
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public int IndexUser { get; set; }
        public bool? IsCondition { get; set; }
        public string EmployeeName { get; set; }
        public string TmTempApprovalId { get; set; }

        public List<TempConditionalApprovalVm> ListConditionalApprovals { get; set; }
    }

    public class TempConditionalApprovalVm
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public int GroupConditionIndex { get; set; }
        public string GroupConditionApproval { get; set; }
        public string ConditionalApproval { get; set; }
        public string TemplateUserApprovalId { get; set; }
        public string EmployeeName { get; set; }

    }

    public class ListUserApprovalVm
    {
        public string TempUserApprovalId { get; set; }
        public string TempApprovalId { get; set; }
    }

    public class TrUpdateStatusApprovalViewModel
    {
        public string RefId { get; set; }
        /// <summary>
        /// 3 = Approve
        /// 4 = Reject
        /// </summary>
        public int StatusApproved { get; set; }
        public string Remarks { get; set; }
        public string Id { get; set; }
        public string DetailLink { get; set; }
    }

    public class TmpUserApprovalCompareVm
    {
        public string EmpBasicInfoId { get; set; }
    }

    public class DoaVm
    {
        public string Id { get; set; }
        public string AssignFrom { get; set; }
        [Required]
        public string AssignTo { get; set; }
        [Required]
        public string FromDate { get; set; }
        [Required]
        public string ToDate { get; set; }
        public bool? IsAssignToApproved { get; set; }
        [Required]
        public string ReasonDoa { get; set; }
        public string ReasonRejectAssign { get; set; }
    }
    public class UserDoaVm
    {
        public string UserAppId { get; set; }
        public string EmployeeId { get; set; }
    }
    public class DoaApprovedVm
    {
        public string Id { get; set; }
        public bool? IsAssignToApproved { get; set; }
        public string ReasonRejectAssign { get; set; }
    }

    public class AzureSetupVm
    {
        [Key, Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [MaxLength(250)]
        public string TenantId { get; set; }

        [MaxLength(250)]
        public string ClientId { get; set; }

        [MaxLength(250)]
        public string EmailAccount { get; set; }

        public string SecurityPword { get; set; }
    }

}
