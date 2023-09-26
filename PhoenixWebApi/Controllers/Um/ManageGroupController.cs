using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Core;
using BusinessLogic.Services;
using BusinessLogic.Services.General;
using CommonTool.JEasyUI.DataGrid;
using DataAccess.PhoenixERP.General;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using PhoenixWebApi.Base;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/ManageGroup")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageGroupController : Controller
    {
        private readonly IManageGroupService _repo;
        public ManageGroupController(IManageGroupService repo)
        {
            _repo = repo;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        [Route("Get/{key}")]
        public async Task<IActionResult> Get(string key = "")
        {
            return Ok(await _repo.GetBySearch(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]GroupViewModel data )
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = new TmGroup();
            if (data.Id == "0")
            {
                model.GroupName = data.GroupName;
                model.CreatedOn = DateTime.Now;
                await _repo.AddAsync(model).ConfigureAwait(false);
                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            else
            {
                model = await _repo.Get(data.Id);
                model.GroupName = data.GroupName;
                model.ModifiedOn = DateTime.Now;
                await _repo.EditAsync(model).ConfigureAwait(false);
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
    }
}