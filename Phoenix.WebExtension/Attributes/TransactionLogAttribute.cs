using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.WebExtension.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.WebExtension.Attributes
{
    //TODO: custom transaction log via single controller for multiple actions

    /// <summary>
    /// Untuk controller normal. Jika pake jquery harus dicustom
    /// </summary>
    public class TransactionLogAttribute : ActionFilterAttribute
    {
        readonly ApiClientFactory client;
        private readonly ExceptionResponse exceptionResponse;
        readonly IHttpContextAccessor accessor;

        public TransactionLogAttribute(ApiClientFactory client, ExceptionResponse exceptionResponse, IHttpContextAccessor accessor)
        {
            this.client = client;
            this.exceptionResponse = exceptionResponse;
            this.accessor = accessor;
        }

        public override async void OnActionExecuted(ActionExecutedContext context)
        {
            //await Task.CompletedTask;
            try
            {
                //if (context.HttpContext?.User?.Identity?.Name == null)
                //    return;

                var descriptor = context?.ActionDescriptor as ControllerActionDescriptor;
                var controller = (Controller)context.Controller;
                var area = context.RouteData.Values["area"];

                var actionName = descriptor?.ActionName;
                var controllerName = descriptor?.ControllerName;
                var requestMethod = context.HttpContext.Request.Method;
                var route = context.HttpContext.Request.GetDisplayUrl();// $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
                var ipAddress = accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                await client.Post(ApiUrl.TransactionLogUrl, new
                {
                    action = actionName,
                    controller = controllerName,
                    area,
                    route,
                    httpMethod = requestMethod,
                    uniqueId = "-",
                    ipAddress
                });

                base.OnActionExecuted(context);
            }
            catch (Exception ex)
            {
                await exceptionResponse.ThrowException(ex);
            }
        }
    }
}
