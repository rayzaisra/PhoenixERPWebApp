using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using System.Collections.Generic;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    [AllowAnonymousRoleAttribute]
    [Route("api/ManageUserAccess")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageUserAccessController : Controller
    {
        private readonly IManageUserAccessService _repo;
        private readonly IManageMenuService _repoMenu;
        public ManageUserAccessController(IManageUserAccessService repo, IManageMenuService repoMenu)
        {
            _repo = repo;
            _repoMenu = repoMenu;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get/{userId}/{groupId}")]
        public async Task<IActionResult> Get(string userId, string groupId)
        {
            //var data = await _repo.GetListByUserId(userId, groupId).ConfigureAwait(false);
            var data = await _repo.GetUserAccessByGroupingMenu(groupId, userId);
            return Ok(data);
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]List<UserAccessViewModel> data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }

            var listModel = new List<TmUserAccess>();
            var no = 1;
            foreach (var item in data)
            {
                if (no == 1)
                {
                    await _repo.DeleteByUserIdAsync(item.UserId).ConfigureAwait(false);
                }
                var model = new TmUserAccess();
                model.Id = Guid.NewGuid().ToString();
                model.MenuId = item.MenuId;
                model.GroupId = item.GroupId;
                model.UserId = item.UserId;
                model.IsRead = item.IsRead;
                model.IsAdd = item.IsAdd;
                model.IsEdit = item.IsEdit;
                model.IsDelete = item.IsDelete;
                listModel.Add(model);
                
                no++;
            }
            await _repo.AddRangeAsync(listModel.ToArray()).ConfigureAwait(false);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await _repo.Get(Id).ConfigureAwait(false);
            await _repo.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetMenuAccessByUniqueName/{uniqueName}")]
        public async Task<IActionResult> GetMenuAccessByUniqueName(string uniqueName)
        {
            var menu = await _repoMenu.GetByUniqueName(uniqueName);
            var data = await _repo.GetById(menu.Id);
            return Ok(data);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetByContoller/{areaName}/{controllerName}")]
        public async Task<IActionResult> GetByContoller(string areaName, string controllerName)
        {
            var data = await _repo.GetByContoller(areaName, controllerName);
            return Ok(data);
        }

    }
}