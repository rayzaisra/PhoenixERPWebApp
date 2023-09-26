using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Phoenix.WebExtension.Exceptions;
using Phoenix.WebExtension.RestApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.WebExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly IMemoryCache memoryCache;
        private readonly ApiClientFactory client;
        private readonly ExceptionResponse exceptionResponse;
        public AuthorizeRoleAttribute(ApiClientFactory client, IMemoryCache memoryCache,  ExceptionResponse exceptionResponse)
        {
            this.client = client;
            this.memoryCache = memoryCache;
            this.exceptionResponse = exceptionResponse;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            if (await IsAllowed(context))
            {
                base.OnActionExecuting(context);
                return;
            }

            var exception = new ForbiddenAccessException();

            throw exception;
        }

        private async Task<bool> IsAllowed(ActionExecutingContext context)
        {
            return true;

            //try
            //{
            //    var descriptor = context?.ActionDescriptor as ControllerActionDescriptor;
            //    var controller = (Controller)context.Controller;

            //    var allowed = controller.GetType().GetCustomAttributes<AllowAnonymousRoleAttribute>(true).Count() > 0;
            //    if (allowed)
            //        return true;

            //    var actionName = descriptor?.ActionName;
            //    var controName = descriptor?.ControllerName;
            //    var areaName = context.RouteData.Values["area"];
            //    //var displayUrl = context.HttpContext.Request.GetDisplayUrl();

            //    var menuArea = descriptor
            //        .ControllerTypeInfo
            //        .GetCustomAttributes(inherit: true)
            //        .FirstOrDefault(x => x.GetType() == typeof(MenuAreaAttribute))
            //        as MenuAreaAttribute;

            //    string menuId = null;
            //    if (menuArea != null)
            //    {
            //        menuId = menuArea.MenuId;
            //        if (menuId == "Form")
            //            return true;
            //    }

            //    if (string.IsNullOrWhiteSpace(menuId))
            //        return false;

            //    var auth = await client.Get<MenuAccess>(ApiUrl.MenuAuthorizeUrl + "/" + menuId);
            //    actionName = actionName.ToLower();

            //    if (actionName.StartsWith("read") ||
            //        actionName.StartsWith("index") ||
            //        actionName.StartsWith("detail") ||
            //        actionName.StartsWith("view") ||
            //        actionName.StartsWith("get"))
            //    {
            //        return auth.IsRead;
            //    }
            //    else if (actionName.StartsWith("edit") ||
            //        actionName.StartsWith("update") ||
            //        actionName.StartsWith("change"))
            //    {
            //        return auth.IsEdit;
            //    }
            //    else if (actionName.StartsWith("submit") ||
            //        actionName.StartsWith("save") ||
            //        actionName.StartsWith("add") ||
            //        actionName.StartsWith("insert"))
            //    {
            //        return auth.IsAdd;
            //    }
            //    else if (actionName.StartsWith("delete") ||
            //        actionName.StartsWith("softdelete") ||
            //        actionName.StartsWith("remove"))
            //    {
            //        return auth.IsDelete;
            //    }

            //    return false;
            //}
            //catch(Exception ex)
            //{
            //    await exceptionResponse.ThrowException(ex);
            //    return false;
            //}
        }
    }

    public class MenuAccess
    {
        public string MenuId { get; set; }
        public bool IsRead { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string MenuName { get; set; }
        public string MenuUnique { get; set; }
        public string MenuLink { get; set; }
    }
}
