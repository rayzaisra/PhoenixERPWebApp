using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageGroupAccessService : IDataService<TmGroupAccess>
    {
        Task<List<TmGroupAccess>> GetByGroupId(string groupId);
        Task<int> DeleteByGroupIdAsync(string groupId);
        Task<List<GroupingMenuViewModel>> GetByGroupingMenu(string groupId);
        Task<int> DeleteByUserIdAsync(string groupId, string userId);
        Task<List<TmGroupAccess>> GetById(string groupId);
        List<EmployeeMappingDivisionViewModel> GetListUserNotHavingGroup(string key);
        Task<List<MappingGroupUserViewModel>> GetListUserHavingGroup();
    }

}
