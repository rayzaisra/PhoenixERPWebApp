using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Models.Um;
using System;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/groupchat")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class GroupChatController : Controller
    {
        private readonly IGroupChattingService _repo;
        private readonly IGroupChattingMemberService _repoMember;
        public GroupChatController(IGroupChattingService repo, IGroupChattingMemberService repoMember)
        {
            _repo = repo;
            _repoMember = repoMember;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        [Route("Get/{key?}")]
        public async Task<IActionResult> Get(string key = "")
        {
            return Ok(await _repo.GetBySearch(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetById/{id?}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _repo.GetByGroupId(id).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Save([FromBody]GroupChatVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }

            var model = new GroupChatting();
            if (data.Id == "0")
            {
                //insert data Group....
                model.Id = Guid.NewGuid().ToString();
                model.GroupName = data.GroupName;
                await _repo.AddAsync(model);

                //insert member...
                foreach (var member in data.Members)
                {
                    var vm = new GroupChattingMember();
                    vm.Id = Guid.NewGuid().ToString();
                    vm.GroupChattingId = model.Id;
                    vm.MemberCode = member.MemberCode;
                    await _repoMember.AddAsync(vm);
                }
                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            else
            {
                //insert data Group....
                model = await _repo.Get(data.Id);
                model.GroupName = data.GroupName;
                await _repo.EditAsync(model);

                //delete member dulu by Group ID nya...
                await _repoMember.DeleteByGroupIdAsync(data.Id);
                foreach (var member in data.Members)
                {
                    var vm = new GroupChattingMember();
                    vm.Id = Guid.NewGuid().ToString();
                    vm.GroupChattingId = data.Id;
                    vm.MemberCode = member.MemberCode;
                    await _repoMember.AddAsync(vm);
                }

                return Ok(new ApiResponse(true, Messages.SuccessEdit));
            }
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repoMember.DeleteSoftByGroupIdAsync(id);
            var model = await _repo.Get(id);
            await _repo.DeleteAsync(model);
            return Ok(new { msg = "success"});
        }

    }
}