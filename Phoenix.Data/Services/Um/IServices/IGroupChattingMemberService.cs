using Phoenix.Data.Models.Um;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IGroupChattingMemberService : IDataService<GroupChattingMember>
    {
        Task<int> DeleteByGroupIdAsync(string groupId);
        Task<int> DeleteSoftByGroupIdAsync(string groupId);
        Task<List<GroupChattingMember>> GetMemberByGroupId(string id);
    }
}
