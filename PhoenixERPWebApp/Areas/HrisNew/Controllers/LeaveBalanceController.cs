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
    public class LeaveBalanceController : Controller
    {
        string url = ApiUrl.LeaveBalanceUrl;
        private readonly ApiClientFactory client;

        public LeaveBalanceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string LeaveRequestTypeId)
        {
            var model = await client.Get<List<GenerateLeaveBalanceByLeaveRequestType>>($"{url}/GenerateByLeaveRequestTypeId/{LeaveRequestTypeId}") ?? new List<GenerateLeaveBalanceByLeaveRequestType>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Generate(string Type)
        {
            var response = await client.PostApiResponse<string>(url, Type);
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "grid" });
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}