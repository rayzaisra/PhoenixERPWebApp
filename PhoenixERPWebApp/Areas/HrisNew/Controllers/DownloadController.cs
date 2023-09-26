using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class DownloadController : Controller
    {
        private readonly ApiClientFactory client;

        public DownloadController(ApiClientFactory client)
        {
            this.client = client;
        }
        
        public async Task<IActionResult> Download(string id)
        {
            var response = await client.Get<DownloadFile>($"{ApiUrl.DownloadFileUrl}/{id}");
            return Json(response, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}