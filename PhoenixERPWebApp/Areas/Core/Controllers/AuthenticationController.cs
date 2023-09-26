using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using CodeMarvel.Infrastructure.Sessions;
using SessionExtensions = CodeMarvel.Infrastructure.Sessions.SessionExtensions;
using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.ModelShared;
using Microsoft.Extensions.Options;
using CodeMarvel.Infrastructure.Options;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Phoenix.WebExtension.Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace PhoenixERPWebApp.Areas.Core.Controllers
{
    [AllowAnonymous]
    [Area("Core")]
    public class AuthenticationController : Controller
    {
        Logger appLogger = LogManager.GetCurrentClassLogger();
        private AppSettingsOption appSetting;
        private readonly IMemoryCache cache;
        public AuthenticationController(IOptions<AppSettingsOption> AppSetting, IMemoryCache cache)
        {
            appSetting = AppSetting.Value;
            this.cache = cache;
        }
        public IActionResult Index()
        {
            appLogger.Debug("Web API URL  : " + appSetting.ApiServerUrl);
            return View();
        }

        [HttpGet]
        public async void Logout()
        {
            cache.Remove(User.Identity.Name + "-Menu-Privilege");
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            foreach (var key in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Append(key, "", new CookieOptions() { Expires = DateTime.Now.AddYears(-1) });
                HttpContext.Response.Cookies.Delete(key);
            }

            HttpContext.Session.Clear();
            Response.Redirect("/Core/Authentication?status=Logout");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IFormCollection Collection)
        {
            cache.Remove(User.Identity.Name + "-Menu-Privilege");
            HttpContext.Session.Clear();
            var Response = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                result.Status.Success = true;
                result.Status.Message = "Unable request to server ";
                try
                {
                    if (Collection.Keys.Contains("username") && Collection.Keys.Contains("password"))
                    {
                        string token = string.Empty;
                        string msToken = string.Empty;
                        string EmployeeBasicInfoId = string.Empty;
                        string BusinessUnitGroup = string.Empty;
                        string BusinessUnitSubgroup = string.Empty;
                        string BusinessUnitDivisi = string.Empty;
                        string BusinessUnitDepartement = string.Empty;
                        string JobTitleId = string.Empty;
                        bool IsFirstTimeLogin = false;
                        using (var _client = new HttpClient())
                        {
                            try
                            {
                                _client.BaseAddress = new Uri(appSetting.ApiServerUrl);
                                var task = _client.PostAsJsonAsync("core/Authentication/authenticate", new
                                {
                                    Username = Collection["username"].ToString(),
                                    Password = Collection["password"].ToString()
                                }).Result;

                                #region old validation
                                //if (task.StatusCode == System.Net.HttpStatusCode.OK)
                                //{
                                //    var response = task.Content.ReadAsAsync<dynamic>().Result;
                                //    if (((bool)((JValue)response.success).Value))
                                //    {
                                //        token = response.data.token;
                                //        msToken = response.data.msToken;
                                //        EmployeeBasicInfoId = response.data.employeeBasicInfoId;
                                //        BusinessUnitGroup = response.data.BusinessUnitGroup;
                                //        BusinessUnitSubgroup = response.data.BusinessUnitSubgroup;
                                //        BusinessUnitDivisi = response.data.BusinessUnitDivisi;
                                //        BusinessUnitDepartement = response.data.BusinessUnitDepartement;
                                //        JobTitleId = response.data.JobTitleId;
                                //        result.Status.Message = "Verified";
                                //        IsFirstTimeLogin = response.data.isFirstTimeLogin;
                                //        var url = "/um/manageuser/changepassword";
                                //        if (!IsFirstTimeLogin) url = "/Core/Home";
                                //        result.Data = new
                                //        {
                                //            redirectTo = url,
                                //            Token = token,
                                //            mstoken = msToken
                                //        };
                                //        HttpContext.Session.SetString(AppConstants.SessionKey, token);
                                //        HttpContext.Session.SetString(AppConstants.EmployeeBasicInfoId, EmployeeBasicInfoId);
                                //        HttpContext.Session.SetString(AppConstants.BusinessUnitGroupId, BusinessUnitGroup);
                                //        HttpContext.Session.SetString(AppConstants.BusinessUnitSubgroupId, BusinessUnitSubgroup);
                                //        HttpContext.Session.SetString(AppConstants.BusinessUnitDivisiId, BusinessUnitDivisi);
                                //        HttpContext.Session.SetString(AppConstants.BusinessUnitDepartementId, BusinessUnitDepartement);
                                //        HttpContext.Session.SetString(AppConstants.JobTitleId, JobTitleId);


                                //    }
                                //    else
                                //    {
                                //        result.Status.Message = "Please check username or password.";
                                //    }

                                //}
                                #endregion

                                #region new validation
                                if (task.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var response = task.Content.ReadAsAsync<dynamic>().Result;
                                    if (((bool)((JValue)response.success).Value))
                                    {
                                        var task2 = _client.PostAsJsonAsync("core/Authentication/CheckIsUserTimesheetLocked", new
                                        {
                                            Username = Collection["username"].ToString(),
                                            Password = Collection["password"].ToString()
                                        }).Result;

                                        if (task2.StatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            token = response.data.token;
                                            msToken = response.data.msToken;
                                            EmployeeBasicInfoId = response.data.employeeBasicInfoId;
                                            BusinessUnitGroup = response.data.BusinessUnitGroup;
                                            BusinessUnitSubgroup = response.data.BusinessUnitSubgroup;
                                            BusinessUnitDivisi = response.data.BusinessUnitDivisi;
                                            BusinessUnitDepartement = response.data.BusinessUnitDepartement;
                                            JobTitleId = response.data.JobTitleId;
                                            result.Status.Message = "Verified";
                                            IsFirstTimeLogin = response.data.isFirstTimeLogin;
                                            var response2 = task2.Content.ReadAsAsync<dynamic>().Result;
                                            if (((bool)((JValue)response2.success).Value))
                                            {
                                                if (response2.data.isLocked == "false")
                                                {
                                                    var url2 = "/pm/Timesheetactivated";
                                                    result.Data = new
                                                    {
                                                        redirectTo = url2,
                                                        Token = token,
                                                        mstoken = msToken
                                                    };
                                                }
                                                else
                                                {
                                                    var url = "/um/manageuser/changepassword";
                                                    if (!IsFirstTimeLogin) url = "/Core/Home";
                                                    result.Data = new
                                                    {
                                                        redirectTo = url,
                                                        Token = token,
                                                        mstoken = msToken
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                var url = "/um/manageuser/changepassword";
                                                if (!IsFirstTimeLogin) url = "/Core/Home";
                                                result.Data = new
                                                {
                                                    redirectTo = url,
                                                    Token = token,
                                                    mstoken = msToken
                                                };

                                            }
                                            HttpContext.Session.SetString(AppConstants.SessionKey, token);
                                            HttpContext.Session.SetString(AppConstants.EmployeeBasicInfoId, EmployeeBasicInfoId ?? string.Empty);
                                            HttpContext.Session.SetString(AppConstants.BusinessUnitGroupId, BusinessUnitGroup ?? string.Empty);
                                            HttpContext.Session.SetString(AppConstants.BusinessUnitSubgroupId, BusinessUnitSubgroup ?? string.Empty);
                                            HttpContext.Session.SetString(AppConstants.BusinessUnitDivisiId, BusinessUnitDivisi ?? string.Empty);
                                            HttpContext.Session.SetString(AppConstants.BusinessUnitDepartementId, BusinessUnitDepartement ?? string.Empty);
                                            HttpContext.Session.SetString(AppConstants.JobTitleId, JobTitleId ?? string.Empty);
                                        }


                                    }
                                    else
                                    {
                                        result.Status.Message = "Please check username or password.";
                                    }

                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                var mes = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                var task = _client.PostAsJsonAsync("core/Authentication/logLogin", new
                                {
                                    Username = mes
                                  
                                }).Result;
                                //appLogger.Error(ex);
                                appLogger.Error(mes);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    using (var _client = new HttpClient())
                    {
                        _client.BaseAddress = new Uri(appSetting.ApiServerUrl);
                        var mes = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        var task = _client.PostAsJsonAsync("core/Authentication/logLogin", new
                        {
                            Username = mes

                        }).Result;
                        appLogger.Error(mes);
                    }
                       
                    //appLogger.Error(ex);
                 
                }

                return result;
            });

            if (Response == null) return null;
            else return Ok(Response);
        }

    }
}