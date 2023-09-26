using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.ApiExtension.Exceptions;
using Phoenix.Data.Models;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private List<TmAuthorization> authorizations;

        private readonly IMemoryCache cache;
        private readonly IManageUserAccessService user;
        //private readonly IConfiguration configuration;
        private readonly DataContext dataContext;
        private readonly IDoaService doa;

        public AuthorizeRoleAttribute(IManageUserAccessService user, IDoaService doa, IMemoryCache cache, DataContext context)
        {
            this.user = user;
            //this.configuration = configuration;
            this.doa = doa;
            this.cache = cache;
            this.dataContext = context;
            LoadSetting();
        }

        private void LoadSetting()
        {
            if (dataContext.GetUserId() == null)
                return;
            ////List<TmAuthorization> authorizations;
            //if (!cache.TryGetValue(nameof(authorizations), out authorizations))
            //{
                
            //    authorizations = dataContext.TmAuthorizations.Where(x => !(x.IsDeleted ?? false)).ToList();
            //    //configuration.GetSection("Authorization").Get<RootObject>();

            //    var options = new MemoryCacheEntryOptions
            //    {
            //        SlidingExpiration = TimeSpan.FromHours(8),
            //        AbsoluteExpiration = DateTime.Today.AddHours(8)
            //    };

            //    cache.Set(nameof(authorizations), authorizations, options);
            //}
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            //user = context.HttpContext.RequestServices.GetService(typeof(IManageUserAccessService)) as IManageUserAccessService;
            if (await IsAllowed(context))
            {
                base.OnActionExecuting(context);
                return;
            }
            else
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var obj = new
                {
                    success = false,
                    message = "You have no authorization to the current access.",
                    statusCode = 403,
                    statusMessage = "Forbidden"
                };
                var result = new JsonResult(obj, settings);
                context.HttpContext.Response.StatusCode = 403;//context.Result = new ForbidResult();//new StatusCodeResult(403);

                context.Result = result;
            }
        }

        private async Task<bool> IsAllowed(ActionExecutingContext context)
        {
            return true;
            //if (dataContext.GetUserId() == null)
            //    return true;

            //bool allowed = false;
            //var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            //if (!descriptor.MethodInfo.IsPublic)
            //    return true;

            //if (context.Controller is ControllerBase)
            //    allowed = (context.Controller as ControllerBase).GetType().GetCustomAttributes<AllowAnonymousRoleAttribute>(true).Count() > 0;
            //else
            //    allowed = (context.Controller as Controller).GetType().GetCustomAttributes<AllowAnonymousRoleAttribute>(true).Count() > 0;

            //if (!allowed)
            //    allowed = descriptor.MethodInfo.GetCustomAttributes<AllowAnonymousRoleAttribute>(inherit: true).Count() > 0;

            //if (allowed)
            //    return true;

            //var actionRole = descriptor
            //    .MethodInfo
            //    .GetCustomAttributes(inherit: true)
            //    .FirstOrDefault(x => x.GetType() == typeof(ActionRoleAttribute)) as ActionRoleAttribute;

            //if (actionRole == null)
            //    return false;

            //var menuId = authorizations
            //    .Where(x => x.ControllerName.Replace("Controller", "", StringComparison.OrdinalIgnoreCase).Equals(descriptor.ControllerName, StringComparison.OrdinalIgnoreCase))
            //    .Select(x => x.MenuId)
            //    .FirstOrDefault();

            //if (string.IsNullOrWhiteSpace(menuId))
            //    return false;

            ////cek privilge user asli (bukan doa)
            //var authorized = await GetPrivilege(actionRole, menuId);
            ////kalo punya otorisasi ga perlu cek DOA
            //if (!authorized)
            //{
            //    //cek apakah user ada di DOA(delegation of authority)
            //    var users = await doa.IsUserInDoa();
            //    if (users?.Count > 0)
            //    {
            //        foreach (var u in users)
            //        {
            //            authorized = await GetPrivilege(actionRole, menuId, u.UserAppId);
            //            if (authorized)
            //                break;
            //        }
            //    }
            //}

            //return authorized;
        }

        private async Task<bool> GetPrivilege(ActionRoleAttribute actionRole, string menuId, string userId = null)
        {
            var auth = await user.GetById(menuId, userId);

            if (auth == null)
                return false;

            switch (actionRole.ActionMethod)
            {
                case ActionMethod.Read:
                    return auth.IsRead;
                case ActionMethod.Add:
                    return auth.IsAdd;
                case ActionMethod.Edit:
                    return auth.IsEdit;
                case ActionMethod.Delete:
                    return auth.IsDelete;
                default:
                    return true;
            }
        }
    }
}
