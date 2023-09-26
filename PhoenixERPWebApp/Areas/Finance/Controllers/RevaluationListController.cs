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

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class RevaluationListController : Controller
    {
        string url = ApiUrl.RevaluationListUrl;
        private readonly ApiClientFactory client;

        public RevaluationListController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new RevaluationList();
            ViewBag.processApprove = false;

            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<RevaluationList>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Rejected);
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            { 
                model.Status = StatusTransactionName.Draft;
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<RevaluationList>>(url) ?? new List<RevaluationList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [HttpPost]
        public async Task<IActionResult> Save(RevaluationList model)
        {
            model.Status = StatusTransactionName.Draft;
            return await Process(model, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(RevaluationList model)
        {
            model.Status = StatusTransactionName.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(RevaluationList model)
        {
            var response = await client.PutApiResponse<RevaluationList>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(RevaluationList model)
        {
            var response = await client.PutApiResponse<RevaluationList>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        private async Task<IActionResult> Process(RevaluationList model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<RevaluationList>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<RevaluationList>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as RevaluationList;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "LogDescription", value = "" } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(RevaluationList model) => Json(await client.DeleteApiResponse<RevaluationList>($"{url}/{model.Id}"));
    }
}