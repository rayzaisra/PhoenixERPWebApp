using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using BusinessLogic;
using BusinessLogic.Core;
using System.Net.Http;
using System.Web.Http;
using NLog;
using CodeMarvel.Infrastructure.Acl;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Services;
using CommonTool;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Phoenix.Data.Attributes;
using Microsoft.Extensions.Caching.Memory;

namespace PhoenixWebApi.Controllers.Core
{
    [AllowAnonymousRoleAttribute]
    [ApiController]
    [Route("api/core/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class AuthenticationController : ControllerBase
    {
        private readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        private readonly IManageUserAppService _repo;
        private readonly IMemoryCache _cache;
        public AuthenticationController(IManageUserAppService repo, IMemoryCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [AllowAnonymousRoleAttribute]
        // POST: api/core/Authentication
        [HttpPost]
        [Route("logLogin")]
        public async Task<IActionResult> logLogin([FromBody] CodeMarvel.Infrastructure.ModelShared.UserCredential Credential)
        {
            var a = Task.Run(() => _repo.InsertLog(Credential.Username)).Result;
            return Ok(new ApiResponse(a, true, Messages.SuccessLogin));

        }

        [AllowAnonymousRoleAttribute]
        // POST: api/core/Authentication
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] CodeMarvel.Infrastructure.ModelShared.UserCredential Credential)
        {
            _cache.Remove("authorizations");
            var dataUser = await _repo.Get(Credential.Username); //Task.Run(() => _repo.Get(Credential.Username)).Result;
            if (dataUser == null)
            {
                return BadRequest(new ApiResponse(true, Messages.d));
            }
            var msToken = "";
            var userExist = false;
            //if (dataUser.IsMsUser == true)
            //{
            //    MsGraphLogin parameter = new MsGraphLogin();
            //    parameter.username = Credential.Username;
            //    parameter.password = Credential.Password;
            //    parameter.client_id = "766203ec-9c5f-4f99-9422-7adc76a30788";
            //    parameter.client_secret = "EsVF5tDpzyMkS9GcjMY9CB2QybeMfbT0Bp0YYLlokAw=";
            //    parameter.grant_type = "password";
            //    parameter.requested_token_use = "on_behalf_of";
            //    parameter.resource = "https://graph.microsoft.com";
            //    parameter.scope = "openid";
            //    RestClient<MsGraphLoginresult> ApiClient = new RestClient<MsGraphLoginresult>();
            //    var retval = ApiClient.GetResponse("https://login.microsoftonline.com/3291f647-acb0-4ac9-b57d-60158629a183/oauth2/token?prompt=consent", parameter);
            //    if (retval != null)
            //    {
            //        msToken = retval.access_token;
            //        userExist = true;
            //    }
            //}
            //else
            //{
            var en = Phoenix.ApiExtension.Helpers.SecurityHelper.EncryptString("$Jg9Vd3mZT");
            var usrPwd = Phoenix.ApiExtension.Helpers.SecurityHelper.DecryptString(dataUser.UserPwd);
            if (usrPwd != Credential.Password)
            {
                return BadRequest(new ApiResponse(true, Messages.d));
            }
            //}

            var dataBu = new Phoenix.Data.Models.BusinessUnitJobLevel();
            if (!string.IsNullOrEmpty(dataUser.EmployeeBasicInfoId))
            {
                dataBu = Task.Run(() => _repo.GetBusinessUnitJobLevel(dataUser.EmployeeBasicInfoId)).Result;
            }
            if (dataBu == null)
            {
                dataBu = new Phoenix.Data.Models.BusinessUnitJobLevel();
                dataBu.BusinessUnitGroupId = "";
                dataBu.BusinessUnitSubgroupId = "";
                dataBu.BusinessUnitDivisionId = "";
                dataBu.BusinessUnitDepartementId = "";
                dataBu.JobTitleId = "";
            }
            var rawSession = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dataUser.AliasName), // AliasName or Full Name
                new Claim(ClaimTypes.Email, dataUser.Email),// Email
                new Claim(ClaimTypes.PrimarySid, dataUser.Id), // Set AppUserId
                new Claim(ClaimTypes.Role, dataUser.GroupId), // Set GroupId
                new Claim(ClaimTypes.NameIdentifier, dataUser.EmployeeBasicInfoId),
                new Claim(ClaimTypes.GroupSid, dataBu.BusinessUnitGroupId??""),
                new Claim(ClaimTypes.PrimaryGroupSid, dataBu.BusinessUnitSubgroupId??""),
                new Claim(ClaimTypes.SerialNumber, dataBu.BusinessUnitDivisionId??""),
                new Claim(ClaimTypes.StreetAddress, dataBu.BusinessUnitDepartementId??""),
                new Claim(ClaimTypes.Locality, dataBu.JobTitleId??""),
                new Claim(ClaimTypes.WindowsUserClaim, rawSession),

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(principal);

            var token = JwtManager.GenerateToken(Credential.Username, new
            {
                AppUserId = dataUser.Id,
                FullName = dataUser.AliasName,
                EmailUser = dataUser.Email,
                GroupAccess = dataUser.GroupId,
                EmployeeId = string.IsNullOrEmpty(dataUser.EmployeeBasicInfoId) ? "" : dataUser.EmployeeBasicInfoId,
                BusinessGroup = dataBu == null ? "" : dataBu.BusinessUnitGroupId,
                BusinessSubGroup = dataBu == null ? "" : dataBu.BusinessUnitSubgroupId,
                BusinessUnitDivisi = dataBu == null ? "" : dataBu.BusinessUnitDivisionId,
                BusinessUnitDepartment = dataBu == null ? "" : dataBu.BusinessUnitDepartementId,
                JobTitleId = dataBu == null ? "" : dataBu.JobTitleId,
                WindowsUserClaim = rawSession
            });


            var dataresult = new
            {
                Token = token,
                MsToken = msToken,
                EmployeeBasicInfoId = dataUser.EmployeeBasicInfoId,
                BusinessGroup = dataBu.BusinessUnitGroupId,
                BusinessSubGroup = dataBu.BusinessUnitSubgroupId,
                BusinessUnitDivisi = dataBu.BusinessUnitDivisionId,
                BusinessUnitDepartment = dataBu.BusinessUnitDepartementId,
                JobTitleId = dataBu.JobTitleId,
                IsFirstTimeLogin = dataUser.IsFirstTimeLogin
            };

            // update isonline nya ...si user
            var vmuser = await _repo.Get(dataUser.Id);
            vmuser.IsOnLine = true;
            vmuser.RawSession = rawSession;
            await _repo.EditAsync(vmuser);

            return Ok(new ApiResponse(dataresult, true, Messages.SuccessLogin));

            //try
            //{
            //    var auth = new Authentication(appLogger);
            //    var token = "";
            //    var mstoken = "";
            //    result.Status.Success = auth.Authenticate(Credential, out token, out mstoken);
            //    if (result.Status.Success)
            //    {
            //        result.Data = new
            //        {
            //            Token = token,
            //            MsToken= mstoken
            //        };
            //    }
            //}
            //catch (Exception ex)
            //{
            //    appLogger.Error(ex.ToString());
            //    result.Status.Message = ex.Message;
            //}

        }

        [AllowAnonymous]
        [HttpGet]
        [ServiceFilter(typeof(JwtAuthenticationAttribute))]
        [Route("validate")]
        public ActionResult<ApiResponse> Validate([FromBody] CodeMarvel.Infrastructure.ModelShared.UserCredential Credential)
        {
            //var result = new ApiResponse();
            //result.Status.Success = true;
            //return result;

            return Ok(new ApiResponse(true, Messages.SuccessLogin));
        }

        [AllowAnonymousRoleAttribute]
        // POST: api/core/Authentication
        [HttpPost]
        [Route("CheckIsUserTimesheetLocked")]
        public async Task<IActionResult> CheckIsUserTimesheetLocked([FromBody] CodeMarvel.Infrastructure.ModelShared.UserCredential Credential)
        {
            var dataUser = await _repo.Get(Credential.Username); //Task.Run(() => _repo.Get(Credential.Username)).Result;

            if (dataUser == null)
            {
                return BadRequest(new ApiResponse(true, Messages.d));
            }

            var dataLocked = Task.Run(() => _repo.GetUserIsLocked(dataUser.EmployeeBasicInfoId)).Result;
            var dataresult = new
            {
                IsLocked = dataLocked

            };
            return Ok(new ApiResponse(dataresult, true, Messages.SuccessLogin));
        }
    }
}
