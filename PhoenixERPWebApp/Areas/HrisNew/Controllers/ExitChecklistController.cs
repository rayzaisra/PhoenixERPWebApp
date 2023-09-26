using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class ExitChecklistController : Controller
    {
        string url = ApiUrl.ExitChecklistnUrl;
        private readonly ApiClientFactory client;

        public ExitChecklistController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = await client.Get<Termination>($"{url}/{Id}");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Termination>>(url) ?? new List<Termination>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJobHandover([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getJobHandoverList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadOfficeManagement([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getOfficeManagementList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadInformationTechnology([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getInformationTechnologyList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadFinance([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getFinanceList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadHumanResource([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getHumanResourceList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadOther([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getOtherList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadFinalHandover([DataSourceRequest] DataSourceRequest request, string terminationId, string employeeBasicInfoId)
        {
            var model = await client.Get<List<ExitChecklist>>($"{url}/getFinalHandoverList/{terminationId}/{employeeBasicInfoId}") ?? new List<ExitChecklist>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [HttpPost]
        public async Task<IActionResult> Submit(Termination model)
        {
            var response = await client.PutApiResponse<Termination>(url, model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }


        [HttpPost]
        public async Task<IActionResult> Save(Termination model)
        {
            model.Status = PhoenixERPWebApp.Models.TerminationExitChecklistStatus.OnProgress;
            var response = await client.PostApiResponse<Termination>(url, model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [AcceptVerbs("Post")]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, ExitChecklist model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}