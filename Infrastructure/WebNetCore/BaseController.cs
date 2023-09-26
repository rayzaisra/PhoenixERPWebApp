using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Sessions;
using CommonTool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CodeMarvel.Infrastructure.WebNetCore
{

    public class BaseController : Controller, IActionFilter
    {
        public NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();
        public UserSession UserSession { get; set; }
        public String CurrentModule { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UserSession = CodeMarvel.Infrastructure.Sessions.SessionManager.UserSession;
            
            if (UserSession == null || string.IsNullOrEmpty(UserSession.Token))
            {
                bool isAjax = Request.Headers["x-requested-with"] == "XMLHttpRequest";
                if (!isAjax)
                {
                    Response.Redirect("/Core/Authentication?status=sessionExpired");
                }
                else
                {

                }

                //bool isAjaxCall = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
                //if (!isAjaxCall)
                //{
                //    Response.Redirect("/Core/Authentication?status=sessionExpired");
                //}

            }
            else
            {
                appLogger.Info("user Session");
                appLogger.Info(JsonConvert.SerializeObject(UserSession));
                ViewBag.FullName = UserSession.FullName;
            }
        }

    }
}
