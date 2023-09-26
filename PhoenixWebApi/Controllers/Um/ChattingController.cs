using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;
using Microsoft.AspNetCore.SignalR;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/chat")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ChattingController : Controller
    {
        private readonly IChatInfoService _repo;
        private readonly IGroupChattingService _repoGroupChat;
        private readonly IGroupChattingMemberService _repoGroupChatMember;
        private readonly IHubContext<server.Hubs.NotificationHub> _hubContext;
        public ChattingController(IChatInfoService repo, IHubContext<server.Hubs.NotificationHub> hubContext, IGroupChattingService repoGroupChat, IGroupChattingMemberService repoGroupChatMember)
        {
            _repo = repo;
            _hubContext = hubContext;
            _repoGroupChat = repoGroupChat;
            _repoGroupChatMember = repoGroupChatMember;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListContact")]
        public async Task<IActionResult> GetListContact()
        {
            var memberCode = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var listContactPersonal = await _repo.GetListContact(memberCode);
            var listTeam = _repo.GetListTeamByMember(memberCode);
            var listGroupChat = await _repoGroupChat.GetGroupChatByMemberCode(memberCode);
            var contact = new
            {
                contactPersoal = listContactPersonal,
                team = listTeam,
                groupChat = listGroupChat
            };

            return Ok(contact);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListChatPersonal/{code?}")]
        public async Task<IActionResult> GetListChatPersonal(string code)
        {
            return Ok(await _repo.GetListChat(code));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListChatTeam/{code?}")]
        public async Task<IActionResult> GetListChatTeam(string code)
        {
            return Ok(await _repo.GetListChatTeam(code));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("AddContactPersonal")]
        public async Task<IActionResult> AddContactPersonal([FromBody]ContactMemberChatVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }

            var collaborateCode = await _repo.AddChatInfo(data);
            var vm = new
            {
                codex = collaborateCode
            };
            return Ok(new ApiResponse(vm,true, Messages.SuccessEdit));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("PostChatTeam")]
        public async Task<IActionResult> PostChatTeam([FromBody]ChatAndCommentVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }

            var memberCode = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var model = new ChattingAndComment();
            model.CollaborateCode = data.CollaborateCode;
            model.MemberCode = memberCode;
            model.MemberName = data.MemberName;
            model.ChatText = data.ChatText;
            model.FileExt = data.FileExt;
            _repo.AddChatAndCommentAsync(model).ConfigureAwait(false).GetAwaiter().GetResult();

            if (data.Type == "T")
            {
                var listTeam = _repo.GetListTeamMemberByJobId(data.CollaborateCode);
                foreach (var team in listTeam)
                {
                    if (team.MemberCode != memberCode)
                        await _hubContext.Clients.All.SendAsync("ReceiveMsgTeam", data.CollaborateCode, team.MemberCode, memberCode, data.MemberName, data.ChatText, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                }
            }
            else if (data.Type == "G") {
                var listMemberInGroup = await _repoGroupChatMember.GetMemberByGroupId(data.CollaborateCode);
                foreach (var team in listMemberInGroup)
                {
                    if (team.MemberCode != memberCode)
                        await _hubContext.Clients.All.SendAsync("ReceiveMsgTeam", data.CollaborateCode, team.MemberCode, memberCode, data.MemberName, data.ChatText, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                }

            }
            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        //[AllowAnonymousRole]
        //[HttpPost]
        //[Route("PostChatFile")]
        //public async Task<IActionResult> PostChatFile([FromForm]ChatAndCommentFileVm data)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Ok(new ApiResponse(false, "data not valid"));
        //    }

        //    var memberCode = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
        //    var model = new ChattingAndComment();
        //    model.CollaborateCode = data.CollaborateCode;
        //    model.MemberCode = memberCode;
        //    model.MemberName = data.MemberName;

        //    model.ChatText = data.ChatText;
        //    model.FileExt = data.FileExt;
        //    await _repo.AddChatAndCommentAsync(model);
        //    return Ok(new ApiResponse(true, Messages.SuccessEdit));
        //}
    }

}