using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageUserAppService : IDataService<TmUserApp>
    {
        Task<BusinessUnitJobLevel> GetBusinessUnitJobLevel(string empId);
        Task<List<TmUserApp>> GetListUserByGroupId(string groupId);
        Task<TmUserApp> CekIdIsReady(string Id);
        Task<List<TmUserApp>> GetBySearch(string key);
        Task<List<EmployeeBasicInfo>> GetDataEmployee(string key);
        Task<List<EmployeeBasicInfo>> GetDataEmployeeSortByName(string key);
        Task<List<TmUserApp>> GetByUserLogin();
        Task<List<TmUserApp>> GetUserForDoa(string userId);
		Task<TmUserApp> GetByEmployeeBasicInfoId(string id);

        Task<string> GetUserIsLocked(string empId);
        Task<bool> IsAllowOpenEncryptEmployeePayroll(string empId, string MenuUnique);
        Task<bool> InsertLog(string message);


    }

    public interface IManageProfileUserService : IDataService<TmUserApp>
    {
        Task<List<VmProfileInfo>> GetProfileInfo(string userid);
    }
}
