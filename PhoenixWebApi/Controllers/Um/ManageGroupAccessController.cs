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
    [Route("api/ManageGroupAccess")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageGroupAccessController : Controller
    {
        private readonly IManageGroupAccessService _repo;
        public ManageGroupAccessController(IManageGroupAccessService repo)
        {
            _repo = repo;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        [Route("Get/{key?}")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _repo.GetByGroupId(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListUserHavingGroup")]
        public async Task<IActionResult> GetListUserHavingGroup()
        {
            return Ok(await _repo.GetListUserHavingGroup().ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListUserNotHavingGroup")]
        [Route("GetListUserNotHavingGroup/{key?}")]
        public async Task<IActionResult> GetListUserNotHavingGroup(string key)
        {
            if (string.IsNullOrEmpty(key)) key = string.Empty;
            return Ok(await Task.FromResult(_repo.GetListUserNotHavingGroup(key)));
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetGroupingMenu/{key?}")]
        public async Task<IActionResult> GetGroupingMenu(string key)
        {
            return Ok(await _repo.GetByGroupingMenu(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]List<GroupAccessViewModel> data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }

            var listModel = new List<TmGroupAccess>();
            var no = 1;
            foreach (var item in data)
            {
                if (no == 1)
                {
                    await _repo.DeleteByGroupIdAsync(item.GroupId).ConfigureAwait(false);
                }
                var model = new TmGroupAccess();
                model.Id = Guid.NewGuid().ToString();
                model.MenuId = item.MenuId;
                model.GroupId = item.GroupId;
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

    }


}