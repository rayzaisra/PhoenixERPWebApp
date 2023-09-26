using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageMenuService : IDataService<TmMenu>
    {
        Task<List<TmMenu>> GetRaw();
        Task<List<TmMenu>> GetBySearch(string key);
        string UserId { get; }
        Task<TmMenu> GetByUniqueName(string uniqueName);

        Task<List<TmMenu>> GetHierachyMenu();
        Task<TmMenu> GetMenuByParentIdAndIndex(string parentId, int idx);
        Task<List<TmMenu>> GetListMenuByParentId(string parentId);
        Task<TmMenu> GetLastIndex(string parentId);
    }

}
