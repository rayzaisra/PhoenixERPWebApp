using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/ManageMenu")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageMenuController : Controller
    {
        private readonly IManageMenuService _repo;
        public ManageMenuController(IManageMenuService repo)
        {
            _repo = repo;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        [Route("Get/{key}")]
        public async Task<IActionResult> Get(string key = "")
        {
            //var data =  Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(this.HttpContext);
            return Ok(await _repo.GetBySearch(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]MenuViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = new TmMenu();
            model.MenuName = data.MenuName.Replace(System.Environment.NewLine, data.MenuName.TrimStart().TrimEnd());
            model.MenuLink = data.MenuLink;
            model.MenuUnique = data.MenuUnique;
            model.ParentId = data.ParentId.Replace(System.Environment.NewLine, data.ParentId.TrimStart().TrimEnd());
            if (data.Id == "0")
            {
                var vm = await _repo.GetLastIndex(model.ParentId);

                model.CreatedOn = DateTime.Now;
                var id = Guid.NewGuid().ToString();
                model.Id = id.Replace(System.Environment.NewLine, id.TrimStart().TrimEnd());
                model.MenuIndex = vm.MenuIndex + 1;
                await _repo.AddAsync(model).ConfigureAwait(false);
                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            else
            {
                data.Id = data.Id.Replace(System.Environment.NewLine, data.Id.TrimStart().TrimEnd());
                var dataMenu = await _repo.Get(data.Id).ConfigureAwait(false);
                dataMenu.MenuName = data.MenuName;
                dataMenu.MenuLink = data.MenuLink;
                dataMenu.MenuUnique = data.MenuUnique;
                dataMenu.ParentId = data.ParentId;
                dataMenu.ModifiedOn = DateTime.Now;
                await _repo.EditAsync(dataMenu).ConfigureAwait(false);
                return Ok(new ApiResponse(true, Messages.SuccessEdit));
            }
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


        //Update New Task....
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetHierachyMenu")]
        public async Task<IActionResult> GetHierachyMenu()
        {
            return Ok(await _repo.GetHierachyMenu().ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("updateindex")]
        public async Task<IActionResult> UpdateIndex([FromBody]MenuIndexVm data)
        {
            var model = await _repo.Get(data.Id);
            if (data.Type == "U")
            {
                var modelNext = await _repo.GetMenuByParentIdAndIndex(data.ParentId, Convert.ToInt32(data.MenuIndex) - 1);
                if (modelNext == null) return Ok(await _repo.GetListMenuByParentId(data.ParentId));
                //change index di model next nya, jadi turun...
                var newIdx = modelNext.MenuIndex + 1;
                modelNext.MenuIndex = newIdx;

                //update next model ke database...
                await _repo.EditAsync(modelNext);

                // update si current model, first get , than cjhange index, than update..
                newIdx = model.MenuIndex - 1;
                model.MenuIndex = newIdx;
                await _repo.EditAsync(model);
            }
            else
            {
                var modelNext = await _repo.GetMenuByParentIdAndIndex(data.ParentId, Convert.ToInt32(data.MenuIndex) + 1);
                if (modelNext == null) return Ok(await _repo.GetListMenuByParentId(data.ParentId));
                //change index di model next nya, jadi naik...
                var newIdx = modelNext.MenuIndex - 1;
                modelNext.MenuIndex = newIdx;

                //update next model ke database...
                await _repo.EditAsync(modelNext);

                // update si current model, first get , than cjhange index, than update..
                newIdx = model.MenuIndex + 1;
                model.MenuIndex = newIdx;
                await _repo.EditAsync(model);

            }
            var list = await _repo.GetListMenuByParentId(data.ParentId);
            return Ok(list);
        }


    }
    public class MenuIndexVm
    {
        public string Id { get; set; }
        public string MenuIndex { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }
    }

}