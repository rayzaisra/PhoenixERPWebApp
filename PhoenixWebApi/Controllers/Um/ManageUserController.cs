using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using Phoenix.Data.RestApi;
using Phoenix.ApiExtension.Helpers;
using System.Linq;
using System.Collections.Generic;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.MsGraphExtention;
using Microsoft.Graph;
using System.Text.RegularExpressions;
using System.Text;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/ManageUser")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class ManageUserController : Controller
    {
        private readonly IManageUserAppService _repo;
        private readonly IManageGroupAccessService _repoGa;
        private readonly IManageUserAccessService _repoUa;
        private readonly IManageAzurePortalSetupService _repoAzure;
        readonly IEmailTemplateService _service;
        private const string codeEmailTemplate = "NEWUSER-REG";
        private readonly IManageProfileUserService _repoProfile;
        public ManageUserController(IManageUserAppService repo, IManageGroupAccessService repoGa, IManageUserAccessService repoUa, IManageAzurePortalSetupService repoAzure, IEmailTemplateService service, IManageProfileUserService repoProfile)
        {
            _repo = repo;
            _repoGa = repoGa;
            _repoUa = repoUa;
            _repoAzure = repoAzure;
            _service = service;
            _repoProfile = repoProfile;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetEmployee")]
        [Route("GetEmployee/{key?}")]
        public async Task<IActionResult> GetEmployee(string key = "")
        {
            return Ok(await _repo.GetDataEmployee(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetEmployeeSortByName")]
        [Route("GetEmployeeSortByName/{key?}")]
        public async Task<IActionResult> GetEmployeeSortByName(string key = "")
        {
            return Ok(await _repo.GetDataEmployeeSortByName(key).ConfigureAwait(false));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetListUserByGroupId/{groupId?}")]
        public async Task<IActionResult> GetListUserByGroupId(string groupId)
        {
            return Ok(await _repo.GetListUserByGroupId(groupId).ConfigureAwait(false));
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
        [Route("GetUserForDoa")]
        public async Task<IActionResult> GetUserForDoa()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            return Ok(await _repo.GetUserForDoa(userId));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody]UserAppViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var model = await _repo.CekIdIsReady(data.Id);

            if (model == null)
            {
                model = new TmUserApp();
                model.Id = data.Id;
                model.AliasName = data.AliasName;
                model.Email = data.Email;
                model.UserPwd = SecurityHelper.EncryptString("Nava12345");
                model.EmployeeBasicInfoId = data.EmployeeBasicInfoId;
                model.GroupId = data.GroupId;
                model.IsMsUser = data.IsMsUser;
                model.CreatedOn = DateTime.Now;
                model.IsFirstTimeLogin = true;
                model.IsOnLine = false;
                model.IsLocked = data.IsLocked;
                model.IsUserNeedTimesheet = data.IsUserNeedTimesheet;
                await _repo.AddAsync(model).ConfigureAwait(false);

                //additional New...atas permintaan Argantha, default user by Group access di user access nya...
                await _repoUa.DeleteByUserIdAsync(data.Id);
                var listModel = new List<TmUserAccess>();
                var listGa = await _repoGa.GetById(data.GroupId);
                foreach (var item in listGa.ToList())
                {
                    var modelUA = new TmUserAccess();
                    modelUA.Id = Guid.NewGuid().ToString();
                    modelUA.MenuId = item.MenuId;
                    modelUA.GroupId = item.GroupId;
                    modelUA.UserId = data.Id;
                    modelUA.IsRead = item.IsRead;
                    modelUA.IsAdd = item.IsAdd;
                    modelUA.IsEdit = item.IsEdit;
                    modelUA.IsDelete = item.IsDelete;
                    listModel.Add(modelUA);
                    
                }
                await _repoUa.AddRangeAsync(listModel.ToArray()).ConfigureAwait(false);
                //send email to new user...
                //1. GET Email Template...
                var template = await _service.Get(codeEmailTemplate);
                template.Body = template.Body.Replace("@user",data.AliasName);
                template.Body = template.Body.Replace("@pwd", "Nava12345");
                var plainText = HTMLToText(template.Body);
                plainText = plainText.Replace("\n", "<br>");
                plainText = plainText.Replace("Nava12345", "<b>Nava12345</b>");

                var modelAzure = _repoAzure.GetOnlyOne().Result;
                var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                List<Recipient> recipients = new List<Recipient>();
                recipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = data.Email
                    }
                });

                Message email = new Message
                {
                    Body = new ItemBody
                    {
                        Content = plainText,
                        ContentType = BodyType.Html
                    },
                    Subject = template.Subject,
                    ToRecipients = recipients
                };
                await graphClient.Me.SendMail(email, null).Request().PostAsync();



                return Ok(new ApiResponse(true, Messages.SuccessAdd));
            }
            else
            {
                model.AliasName = data.AliasName;
                model.Email = data.Email;
                model.EmployeeBasicInfoId = data.EmployeeBasicInfoId;
                model.GroupId = data.GroupId;
                model.IsMsUser = data.IsMsUser;
                model.IsUserNeedTimesheet = data.IsUserNeedTimesheet;
                model.IsLocked = data.IsLocked;
                model.ModifiedOn = DateTime.Now;
                model.IsDeleted = false;
                //model.UserPwd = SecurityHelper.EncryptString("Nava12345");
                await _repo.EditAsync(model).ConfigureAwait(false);

                //additional New...atas permintaan Argantha, default user by Group access di user access nya...
                await _repoUa.DeleteByUserIdAsync(data.Id);// delete UserAccess By UserId , di delete user access di group sebelum nya...
                var listModel = new List<TmUserAccess>();
                var listGa = await _repoGa.GetById(data.GroupId);
                foreach (var item in listGa.ToList())
                {
                    var modelUA = new TmUserAccess();
                    modelUA.Id = Guid.NewGuid().ToString();
                    modelUA.MenuId = item.MenuId;
                    modelUA.GroupId = item.GroupId;
                    modelUA.UserId = data.Id;
                    modelUA.IsRead = item.IsRead;
                    modelUA.IsAdd = item.IsAdd;
                    modelUA.IsEdit = item.IsEdit;
                    modelUA.IsDelete = item.IsDelete;
                    listModel.Add(modelUA);
                   
                }
                await _repoUa.AddRangeAsync(listModel.ToArray()).ConfigureAwait(false);

                var template = await _service.Get(codeEmailTemplate);
                template.Body = template.Body.Replace("@user", data.AliasName);
                template.Body = template.Body.Replace("@pwd", "Nava12345");
                var plainText = HTMLToText(template.Body);
                plainText = plainText.Replace("\n", "<br>");
                plainText = plainText.Replace("Nava12345", "<b>Nava12345</b>");

                var modelAzure = _repoAzure.GetOnlyOne().Result;
                var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                List<Recipient> recipients = new List<Recipient>();
                recipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = data.Email
                    }
                });

                Message email = new Message
                {
                    Body = new ItemBody
                    {
                        Content = plainText,
                        ContentType = BodyType.Html
                    },
                    Subject = template.Subject,
                    ToRecipients = recipients
                };
                await graphClient.Me.SendMail(email, null).Request().PostAsync();
                return Ok(new ApiResponse(true, Messages.SuccessEdit));
            }
        }



        [AllowAnonymousRole]
        [HttpPost]
        [Route("ChangePword")]
        public async Task<IActionResult> ChangePword([FromBody]ChangePwordVm data)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var model = await _repo.Get(userId);
            model.UserPwd = SecurityHelper.EncryptString(data.newpwd);
            model.ModifiedOn = DateTime.Now;
            model.IsFirstTimeLogin = false;
            model.IsOnLine = false;
            await _repo.EditAsync(model);

            return Ok(new ApiResponse(true, Messages.SuccessEdit));
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("BulkSave")]
        public async Task<IActionResult> BulkSave([FromBody]List<UserAppViewModel> listData)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse(false, "data not valid"));
            }
            foreach (var data in listData)
            {
                var model = await _repo.CekIdIsReady(data.Id);

                if (model == null)
                {
                    model = new TmUserApp();
                    model.Id = data.Id;
                    model.AliasName = data.AliasName;
                    model.Email = data.Email;
                    model.UserPwd = SecurityHelper.EncryptString("Nava12345");
                    model.EmployeeBasicInfoId = data.EmployeeBasicInfoId;
                    model.GroupId = data.GroupId;
                    model.IsMsUser = data.IsMsUser;
                    model.IsLocked = data.IsLocked;
                    model.IsUserNeedTimesheet = data.IsUserNeedTimesheet;
                    model.CreatedOn = DateTime.Now;
                    model.IsFirstTimeLogin = true;
                    model.IsOnLine = false;
                    await _repo.AddAsync(model).ConfigureAwait(false);

                    //additional New...atas permintaan Argantha, default user by Group access di user access nya...
                    await _repoUa.DeleteByUserIdAsync(data.Id);
                    var listGa = await _repoGa.GetById(data.GroupId);
                    foreach (var item in listGa.ToList())
                    {
                        var modelUA = new TmUserAccess();
                        modelUA.Id = Guid.NewGuid().ToString();
                        modelUA.MenuId = item.MenuId;
                        modelUA.GroupId = item.GroupId;
                        modelUA.UserId = data.Id;
                        modelUA.IsRead = item.IsRead;
                        modelUA.IsAdd = item.IsAdd;
                        modelUA.IsEdit = item.IsEdit;
                        modelUA.IsDelete = item.IsDelete;
                        await _repoUa.AddAsync(modelUA).ConfigureAwait(false);
                    }

                    //send email to new user...
                    var template = await _service.Get(codeEmailTemplate);
                    template.Body = template.Body.Replace("@user", data.AliasName);
                    template.Body = template.Body.Replace("@pwd", "Nava12345");
                    var plainText = HTMLToText(template.Body);
                    plainText = plainText.Replace("\n", "<br>");
                    plainText = plainText.Replace("Nava12345", "<b>Nava12345</b>");

                    var modelAzure = _repoAzure.GetOnlyOne().Result;
                    var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                    var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                    var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
                    List<Recipient> recipients = new List<Recipient>();
                    recipients.Add(new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = data.Email
                        }
                    });

                    Message email = new Message
                    {
                        Body = new ItemBody
                        {
                            Content = plainText,
                            ContentType = BodyType.Html
                        },
                        Subject = template.Subject,
                        ToRecipients = recipients
                    };
                    await graphClient.Me.SendMail(email, null).Request().PostAsync();

                }
                else
                {
                    model.AliasName = data.AliasName;
                    model.Email = data.Email;
                    model.EmployeeBasicInfoId = data.EmployeeBasicInfoId;
                    model.GroupId = data.GroupId;
                    model.IsMsUser = data.IsMsUser;
                    model.IsLocked = data.IsLocked;
                    model.IsUserNeedTimesheet = data.IsUserNeedTimesheet;
                    model.ModifiedOn = DateTime.Now;
                    model.IsDeleted = false;
                    //model.UserPwd = SecurityHelper.EncryptString("Nava12345");
                    await _repo.EditAsync(model).ConfigureAwait(false);
                    //additional New...atas permintaan Argantha, default user by Group access di user access nya...
                    await _repoUa.DeleteByUserIdAsync(data.Id);// delete UserAccess By UserId , di delete user access di group sebelum nya...
                    var listGa = await _repoGa.GetById(data.GroupId);
                    foreach (var item in listGa.ToList())
                    {
                        var modelUA = new TmUserAccess();
                        modelUA.Id = Guid.NewGuid().ToString();
                        modelUA.MenuId = item.MenuId;
                        modelUA.GroupId = item.GroupId;
                        modelUA.UserId = data.Id;
                        modelUA.IsRead = item.IsRead;
                        modelUA.IsAdd = item.IsAdd;
                        modelUA.IsEdit = item.IsEdit;
                        modelUA.IsDelete = item.IsDelete;
                        await _repoUa.AddAsync(modelUA).ConfigureAwait(false);
                    }
                }
            }
            return Ok(new ApiResponse(true, Messages.SuccessAdd));

        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("Delete/{Id?}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var model = await _repo.Get(Id).ConfigureAwait(false);
            await _repo.DeleteSoftAsync(model);
            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("DeleteFromGroup/{groupId?}/{userId?}")]
        public async Task<IActionResult> DeleteFromGroup(string groupId, string userId)
        {
            await _repoUa.DeleteByUserIdAsync(userId).ConfigureAwait(false);
            var model = await _repo.Get(userId);
            model.GroupId = string.Empty;
            await _repo.EditAsync(model).ConfigureAwait(false);

            return Ok(new ApiResponse(true, Messages.SuccessDelete));
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("IsHCAdmin")]
        public async Task<IActionResult> IsHCAdmin()
        {
            var group = await _repo.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            return Ok(HCAdmin);
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("IsAllowOpenEncryptEmployeePayroll/{Id?}")]
        public async Task<IActionResult> IsAllowOpenEncryptEmployeePayroll(string Id)
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var result = await _repo.IsAllowOpenEncryptEmployeePayroll(userId,Id);
            return Ok(result);
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("IsAdministrator")]
        public async Task<IActionResult> IsAdministrator()
        {
            var group = await _repo.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.SuperAdmin).Count() > 0;
            return Ok(HCAdmin);
        }

        /// <summary>
        /// Kebutuhan untuk detail form request....
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetByUserLogin")]
        public async Task<IActionResult> GetByUserLogin()
        {
            var group = await _repo.GetByUserLogin();
            return Ok(new ApiResponse(group));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("setlogout")]
        public async Task<IActionResult> SetLogout()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var model = await _repo.Get(userId);
            model.IsOnLine = false;
            await _repo.EditAsync(model);
            return Ok(new ApiResponse(true, Messages.SucessSave));
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetProfileInfo")]
        public async Task<IActionResult> GetProfileInfo()
        {
            var userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
            var model = await _repoProfile.GetProfileInfo(userId);
            return Ok(model.FirstOrDefault());
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("CheckRawSession")]
        public async Task<IActionResult> CheckRawSession()
        {
            string rawSession, userId;
            TmUserApp model = new TmUserApp();
            var vm = new VmRawSession();
            try
            {
                rawSession = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetRawSession(HttpContext);
                userId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetUserId(HttpContext);
                model = await _repo.Get(userId);
            }
            catch 
            {
                rawSession = "";
            }
            
            if (string.IsNullOrEmpty(rawSession))
            {
                vm.IsSame = false;
                vm.Msg = "Your session is expired.";
            } else if (model.RawSession.ToLower() != rawSession.ToLower())
            {
                vm.IsSame = false;
                vm.Msg = "Somebody login with this account.";
            }
            else {
                vm.IsSame = true;
            }
            return Ok(vm);
        }


        [AllowAnonymousRole]
        [HttpGet]
        [Route("ResetPasswordUser/{id}")]
        public async Task<IActionResult> ResetPasswordUser(string id)
        {
            var model = await _repo.Get(id);
            //Update Data Password User...
            model.UserPwd = SecurityHelper.EncryptString("Nava12345");
            await _repo.EditAsync(model);

            //Send email TO User Reset..
            var template = await _service.Get("RSETPWD");
            template.Body = template.Body.Replace("@user", model.AliasName);
            template.Body = template.Body.Replace("@pwd", "Nava12345");
            var plainText = HTMLToText(template.Body);
            plainText = plainText.Replace("\n", "<br>");
            plainText = plainText.Replace("Nava12345", "<b>Nava12345</b>");

            var modelAzure = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
            var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            List<Recipient> recipients = new List<Recipient>();
            recipients.Add(new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = model.Email
                }
            });

            Message email = new Message
            {
                Body = new ItemBody
                {
                    Content = plainText,
                    ContentType = BodyType.Html
                },
                Subject = template.Subject,
                ToRecipients = recipients
            };
            await graphClient.Me.SendMail(email, null).Request().PostAsync();

            return Ok(new ApiResponse(true, Messages.SucessSave));
        }

        public static string HTMLToText(string HTMLCode)
        {
            // Remove new lines since they are not visible in HTML
            HTMLCode = HTMLCode.Replace("\n", " ");

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

            // Remove HEAD tag
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
              , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;","&#39;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "\'" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            //sbHTML = sbHTML.Replace("</p>", "</p><br>");
            //sbHTML = sbHTML.Replace("</strong>", "</strong><br>");
            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p>", "\n<p>");
            sbHTML.Replace("<p ", "\n<p ");

            // Finally, remove all HTML tags and return plain text
            return System.Text.RegularExpressions.Regex.Replace(
              sbHTML.ToString(), "<[^>]*>", "");
        }
    }

    public class VmRawSession
    {
        public bool IsSame { get; set; }
        public string Msg { get; set; }
    }
}