using CodeMarvel.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using CodeMarvel.Infrastructure.Acl;
using CommonTool;
using Newtonsoft.Json;
using System.Linq;


namespace PhoenixWebApi.Base
{
    //[Authorize]
    [ServiceFilter(typeof(JwtAuthenticationAttribute))]
    public class ApiBaseController : ControllerBase, IActionFilter
    {
        private static string tokenBearer = "Bearer ";
        protected NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();
        protected ConnectionStringOption optionConnectionString;
        protected DataContext AppUserData { get; private set; }

        [NonAction]
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            optionConnectionString = context.HttpContext.RequestServices.GetService<ConnectionStringOption>();
            var SessionContext = CodeMarvel.Infrastructure.Sessions.SessionManager.UserSession;
            if (SessionContext == null)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
            AppUserData = new DataContext(SessionContext.AppUserId, SessionContext.AppUserIPAddress, SessionContext);
            //AppUserData.OrganizationId = '9EE4834225E334380667DDCF2F60F6DA';
            //Microsoft.Extensions.Primitives.StringValues Token;

            //if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out Token))
            //{
            //    var userToken = Token.ToString();

            //    if(!string.IsNullOrEmpty(userToken) && userToken.Length >= tokenBearer.Length)
            //    {
            //        var TokenData = JwtManager.GetPrincipal(userToken.Substring(tokenBearer.Length));
            //        if (TokenData != null)
            //        {
            //            var SessionContext = JsonConvert.DeserializeObject<CodeMarvel.Infrastructure.Sessions.UserSession>(TokenData.Claims.ToList()[1].Value);
            //            AppUserData = new DataContext(SessionContext.AppUserId, SessionContext.AppUserIPAddress);
            //        }
            //    }

            //    appLogger.Debug("Token : " + Token.ToString());
            //}

            appLogger.Debug("Masuk ActionFilter From API BaseController");
        }

        protected string EnsureCorrectFilename(string filename)
        {
            try
            {
                if (!string.IsNullOrEmpty(filename) && filename.Contains("\\"))
                    filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
            }

            return filename;
        }
    }
}
