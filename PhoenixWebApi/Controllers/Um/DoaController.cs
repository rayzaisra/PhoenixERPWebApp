using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Models;
using System;
using Phoenix.Data.Attributes;
using PhoenixWebApi.Services;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/doa")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class DoaController : Controller
    {
        private readonly IDoaService _repo;
        private readonly INotificationCenter _notify;
        public DoaController(IDoaService repo, INotificationCenter notify)
        {
            _repo = repo;
            _notify = notify;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListRequest")]
        public async Task<IActionResult> GetListRequest()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetRequestByUserId(userId).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListNeedApproval")]
        public async Task<IActionResult> GetListNeedApproval()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetNeedApprovalByUserId(userId).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetById/{id?}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _repo.Get(id).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> Save([FromBody]DoaVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = new Doa();
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            if (data.Id == "0")
            {
                model.AssignFrom = userId;
                model.AssignTo = data.AssignTo;
                model.FromDate = Convert.ToDateTime(data.FromDate);
                model.ToDate = Convert.ToDateTime(data.ToDate);
                model.ReasonDoa = data.ReasonDoa;
                model.CreatedOn = DateTime.Now;
                await _repo.AddAsync(model);
                await Task.Run(() => _notify.AddNotification("hr.doa.need-approval", data.AssignTo, "HRIS"));

                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            else
            {
                model = await _repo.Get(data.Id);
                model.AssignTo = data.AssignTo;
                model.FromDate = Convert.ToDateTime(data.FromDate);
                model.ToDate = Convert.ToDateTime(data.ToDate);
                model.ReasonDoa = data.ReasonDoa;
                model.ModifiedOn = DateTime.Now;
                await _repo.EditAsync(model);
                if (data.AssignTo != model.AssignTo)
                    await Task.Run(() => _notify.AddNotification("hr.doa.need-approval", data.AssignTo, "HRIS"));

                return Ok(new ApiResponse(true, Messages.SuccessEdit));
            }
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Approved")]
        public async Task<IActionResult> Approved([FromBody]DoaApprovedVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            if (data.IsAssignToApproved == false && string.IsNullOrEmpty(data.ReasonRejectAssign.Trim()))
            {
                return BadRequest(new ApiResponse(false, "reason rejected cannot empty"));
            }
            var model = await _repo.Get(data.Id);

            if (data.IsAssignToApproved == false && !string.IsNullOrEmpty(data.ReasonRejectAssign.Trim()))
                model.ReasonRejectAssign = data.ReasonRejectAssign;

            model.IsAssignToApproved = data.IsAssignToApproved;
            model.ModifiedOn = DateTime.Now;
            await _repo.EditAsync(model).ConfigureAwait(false);
            await Task.Run(() => _notify.AddNotification("hr.doa.feedback-assignment", model.AssignFrom, "HRIS"));

            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Delete/{id?}")]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _repo.Get(id);
            return Ok(await _repo.DeleteAsync(model).ConfigureAwait(false));
        }

    }
}