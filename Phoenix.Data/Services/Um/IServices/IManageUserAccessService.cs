using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageUserAccessService : IDataService<TmUserAccess>
    {
        Task<TmUserAccess> GetDataByUserId(string userId);
        Task<List<TmUserAccess>> GetListByUserId(string userId, string groupId);
        Task<TmUserAccessViewModel> GetById(string menuId, string userId = "");
        Task<int> DeleteByUserIdAsync(string userId);
        Task<List<GroupingMenuUserAccessViewModel>> GetUserAccessByGroupingMenu(string groupId, string userId);
        Task<TmUserAccessViewModel> GetByContoller(string areaName, string controllerName);
    }

}
