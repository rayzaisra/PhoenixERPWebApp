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
    public class FinancePeriodController : Controller
    {
        string url = ApiUrl.FinancePeriodUrl;
        private readonly ApiClientFactory client;

        public FinancePeriodController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            ViewBag.EmployeeId = requestor?.EmployeeId;
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new FinancePeriod();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<FinancePeriod>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Rejected);
                ViewBag.IsRequetorReOpen = ViewBag.processApprove == false && model.Status == StatusTransactionName.Close;
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.Status = StatusTransactionName.Draft;
            }

            //var date = await client.Get<DateTime>($"{url}/MaxDate");
            //ViewBag.MaxDate = date.ToString("yyyy-MM-dd");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<FinancePeriod>>(url) ?? new List<FinancePeriod>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPut]
        public async Task<IActionResult> Approve(FinancePeriod model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<FinancePeriod>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(FinancePeriod model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<FinancePeriod>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(FinancePeriod model)
        {
            model.IsClose = true;
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model);
        }

        [HttpPut]
        public async Task<IActionResult> Reopen(FinancePeriod model)
        {
            model.IsClose = false;
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(FinancePeriod model)
        {
            model.IsClose = false;
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model);
        }

        private async Task<IActionResult> Process(FinancePeriod model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<FinancePeriod>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<FinancePeriod>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(FinancePeriod model)
        {
            var response = await client.DeleteApiResponse<FinancePeriod>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}