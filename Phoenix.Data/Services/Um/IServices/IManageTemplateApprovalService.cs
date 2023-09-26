using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Phoenix.Data.Services.Um.ManageTemplateApprovalService;

namespace Phoenix.Data.Services.Um
{
    public interface IManageTemplateApprovalService : IDataService<TmTemplateApproval>
    {
        Task<List<TmTemplateApproval>> GetBySubGroup(string groupId, string divisionId);
        Task<List<TmTemplateApproval>> GetByCategory(int typeId);
        Task<List<JobTitle>> GetJobTitlesByDivisionId(string id);
        Task<List<BusinessUnit>> GetDivision();
        Task<BusinessUnitJobLevel> GetBussinessUnitJobLevel(string jobTitleId, string divisionId);
        Task<TmTemplateApproval> GetByMenuId(string groupId, string divisionId, string menuId, string jobTitleId);
        Task<TmTemplateApproval> GetById(string Id);
        Task<TmTemplateApproval> GetByBudgeting(string groupId, string divisionId, string menuId, decimal? budget);
        Task<VmUser> GetSpecialCaseUser(string id);
        Task<bool> AddNotification(string notificationTypeId, string userId, string moduleName, string refid = null, bool? isWeb = null, bool? isEmail = null, string requestby = null, string linkNotifCustom = null);
        Task<bool> CheckNotificationIsExists(string notificationTypeId, string userId, string itemId);
        Task<bool> AddNotificationAfterApproval(string userId, string refid, int? status, string linkCustom = null);
        Task<bool> SendEmail(string mailTo, string mailFrom, string mailSubject, string mailBody);
    }
}
