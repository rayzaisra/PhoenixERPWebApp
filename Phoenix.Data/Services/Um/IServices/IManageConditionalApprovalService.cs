using Phoenix.Data.Models.Um;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageConditionalApprovalService : IDataService<TmConditionUserApproval>
    {
        Task<int> DeleteByUserApprovalTempId(string id);
    }
}
