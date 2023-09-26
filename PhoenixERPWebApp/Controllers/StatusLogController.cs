using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    public class StatusLogController : Controller
    {
        string url = ApiUrl.StatusLogUrl;
        private readonly ApiClientFactory client;

        public StatusLogController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<StatusLog>>($"{url}/{id}") ?? new List<StatusLog>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}