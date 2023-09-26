using Phoenix.Data.Models;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IDoaService : IDataService<Doa>
    {
        Task<List<Doa>> GetRequestByUserId(string userId);
        Task<List<Doa>> GetNeedApprovalByUserId(string userId);
        Task<List<UserDoaVm>> IsUserInDoa();
    }

}
