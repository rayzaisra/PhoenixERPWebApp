using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using System;
using System.Threading.Tasks;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/mapfod")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class MapFolderOneDriveController : Controller
    {
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        private readonly IMappingFolderOneDriveService _repo;
        public MapFolderOneDriveController(IMappingFolderOneDriveService repo, IManageAzurePortalSetupService repoAzure)
        {
            _repo = repo;
            _repoAzure = repoAzure;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getdriveitem")]
        [Route("getdriveitem/{folder?}")]
        public async Task<IActionResult> GetListDriveItem(string folder = "")
        {
            if (!string.IsNullOrEmpty(folder))
                folder = folder.Replace('*', '/');

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            //AMBIL SEMUA FILE DAN FOLDER => DI ROOT USER YANG LOGIN
            if (string.IsNullOrEmpty(folder))
                return Ok(await graphClient.Me.Drive.Root.Children.Request().GetAsync());
            else
                return Ok(await graphClient.Me.Drive.Root.ItemWithPath("/" + folder).Children.Request().GetAsync());

        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("finddriveitem")]
        [Route("finddriveitem/{folder?}")]
        public async Task<IActionResult> FindDriveItem(string folder = "")
        {
            if (!string.IsNullOrEmpty(folder))
                folder = "{" + folder + "}";

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            if (string.IsNullOrEmpty(folder))
                return Ok(await graphClient.Me.Drive.Root.Children.Request().GetAsync());
            else
                return Ok(await graphClient.Me.Drive.Search(folder).Request().GetAsync());
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getuserlist/{id?}")]
        [Route("getuserlist/{id?}/{key?}")]
        public async Task<IActionResult> GetUserList(string id,string key = "")
        {
            return Ok(await _repo.GetUserList(id,key));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getgrouplist/{id?}")]
        [Route("getgrouplist/{id?}/{key?}")]
        public async Task<IActionResult> GetGroupList(string id, string key = "")
        {
            return Ok(await _repo.GetGroupList(id, key));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getbyitemid/{id?}")]
        public async Task<IActionResult> GetByItemId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Item Id cannot empty");

            return Ok(await _repo.GetByItemId(id));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("removeall/{id?}")]
        public async Task<IActionResult> RemoveAllMappingByItemId(string id)
        {
            return Ok(await _repo.DeleteAllMappingByItemId(id));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("removebyitemandref/{id?}/{refid?}")]
        public async Task<IActionResult> RemoveByItemIdAndRefId(string id,string refid)
        {
            return Ok(await _repo.DeleteByItemIdAndRefId(id,refid));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("removebymappingid/{id?}")]
        public async Task<IActionResult> RemoveByMappingId(string id)
        {
            var entity = await _repo.Get(id);
            return Ok(await _repo.DeleteSoftAsync(entity));
        }


        [AllowAnonymousRole]
        [HttpPost]
        [Route("AddMember")]
        public async Task<IActionResult> AddMember([FromBody]VmAddFod data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = new TrMappingFolderOneDrive();
            model.Id = Guid.NewGuid().ToString();
            model.ItemId = data.ItemId;
            model.RefId = data.RefId;
            model.RefType = (RefTypeMapping)Enum.ToObject(typeof(RefTypeMapping), data.RefType);
            model.CreatedOn = DateTime.Now;
            model.IsActive = true;
            await _repo.AddAsync(model).ConfigureAwait(false);
            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
    }
}