using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Data.Attributes;
using Phoenix.ApiExtension.Helpers;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/ManageApsetup")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageAzureSetupController : Controller
    {
        private readonly IManageAzurePortalSetupService _repo;
        public ManageAzureSetupController(IManageAzurePortalSetupService repo)
        {
            _repo = repo;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _repo.Get());
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _repo.Get(id));
        }
        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]AzureSetupVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = new AzurePortalSetup();
            if (data.Id == "0")
            {
                model.TenantId = data.TenantId;
                model.ClientId = data.ClientId;
                model.EmailAccount = data.EmailAccount;
                model.SecurityPword = SecurityHelper.EncryptString(data.SecurityPword);
                await _repo.AddAsync(model).ConfigureAwait(false);
                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            else
            {
                model = await _repo.Get(data.Id);
                model.TenantId = data.TenantId;
                model.ClientId = data.ClientId;
                model.EmailAccount = data.EmailAccount;
                model.SecurityPword = SecurityHelper.EncryptString(data.SecurityPword);
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