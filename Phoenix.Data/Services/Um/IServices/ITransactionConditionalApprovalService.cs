using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface ITransactionConditionalApprovalService : IDataService<TrConditionUserApproval>
    {
        Task<int> DeleteByUserApprovalTempId(string id);
        Task<List<TrConditionUserApproval>> GetByUserApprovalId(string id);
        Task<int> UpdateNextApproval(string userApprovalId);
    }
}
