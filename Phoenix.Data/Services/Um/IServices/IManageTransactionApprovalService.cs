using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IManageTransactionApprovalService : IDataService<TrTemplateApproval>
    {
        Task<List<TrTemplateApproval>> GetBySubGroup(string groupId, string divisionId);
        Task<List<TemplateApprovalVm>> GetByCategory(int typeId);
        Task<TrTemplateApproval> GetByRefId(string id);
        Task<List<TrUserApproval>> GetListApprovalByRefId(string id);
        Task<int> DeleteRefTable(string tableName, string refId);
        Task<List<TrTemplateApproval>> GetListApprovalByEmpId(string id);
        Task<TrTemplateApproval> GetByRefIdForUpdate(string id);
        bool IsCurrentApproval(string id);
    }

}
