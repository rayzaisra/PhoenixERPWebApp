using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using Phoenix.WebExtension.Extentions;

namespace Phoenix.WebExtension.Attributes
{
    public class MenuAuthorization
    {
        private readonly ApiClientFactory client;

        public MenuAuthorization(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<TmUserAccessViewModel> GetPrivilege(string areaName, string controllerName)
        {
            var model = await client.Get<TmUserAccessViewModel>($"{ApiUrl.ManageUserAccessUrl}/GetByContoller/{areaName}/{controllerName}") ?? new TmUserAccessViewModel();
            return model;
        }
    }

    public class TmUserAccessViewModel
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