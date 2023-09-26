using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface IChatInfoService : IDataService<ChatInfo>
    {
        Task<List<ChatInfo>> GetListContact(string memberCode);
        Task<List<ChattingAndComment>> GetListChat(string code);
        Task<int> AddChatAndCommentAsync(ChattingAndComment entity);
        Task<string> AddChatInfo(ContactMemberChatVm entity);
        Task<List<ChattingAndComment>> GetListChatTeam(string collaborateCode);
        List<TeamInfoVm> GetListTeamByMember(string memberCode);
        List<ListTeamMember> GetListTeamMemberByJobId(string jobId);
    }
}
