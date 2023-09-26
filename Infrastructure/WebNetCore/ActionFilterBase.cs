using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
namespace CodeMarvel.Infrastructure.WebNetCore
{
    public class ActionFilterBase : IActionFilter
    {
        public NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Microsoft.Extensions.Primitives.StringValues HeaderAutorization;
            //if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            //{
            //    context.HttpContext.Request.Headers.TryGetValue("Authorization", out HeaderAutorization);
                
            //}
            appLogger.Debug("Global Action Filters");
            // do something before the action executes
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
