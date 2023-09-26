using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IGroupChattingService : IDataService<GroupChatting>
    {
        Task<List<GroupChatting>> GetBySearch(string key);
        Task<GroupChatVm> GetByGroupId(string id);
        Task<List<GroupChatting>> GetGroupChatByMemberCode(string memberCode);
    }
}
