using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface ITransactionUserApprovalService : IDataService<TrUserApproval>
    {
        Task<List<TrUserApproval>> GetByTemplateId(string id);
        Task<TrUserApproval> GetByTemplateSingleId(string id);
        Task<int> UpdateNextApproval(string templateAppId, int indexUser);
        Task<TrUserApproval> GetCurrentApproval(string approvalId);
        Task<TrUserApproval> GetUserApprovalByTemplateIdAndEmpId(string trApprovalId, string empId);
        Task<List<AppIdConvert>> GetAppIdFromReferenceId(string referenceId);
    }
}
