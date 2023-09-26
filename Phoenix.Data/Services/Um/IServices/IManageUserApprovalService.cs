using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageUserApprovalService : IDataService<TmTemplateUserApproval>
    {
        Task<List<TmTemplateUserApproval>> GetByTemplateId(string id);
        Task<int> DeleteByTemplateIdAsync(string idTemp, string empId);
    }
}
