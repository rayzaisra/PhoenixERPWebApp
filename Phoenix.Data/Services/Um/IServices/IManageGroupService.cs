using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageGroupService : IDataService<TmGroup>
    {
        Task<List<TmGroup>> GetBySearch(string key);
    }

}
