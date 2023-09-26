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
    [Area(nameof(HrisNew))]
    public class BusinessUnitController : Controller
    {
        string url = ApiUrl.BusinessUnitUrl;
        private readonly ApiClientFactory client;

        public BusinessUnitController(ApiClientFactory client)
        {
            this.client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var model = await client.Get<BusinessUnit>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}