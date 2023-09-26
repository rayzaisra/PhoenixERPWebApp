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
    public class FinancialYearController : Controller
    {
        string url = ApiUrl.FinancialYearUrl;
        private readonly ApiClientFactory client;

        public FinancialYearController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new FinancialYear();
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<FinancialYear>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Rejected);
                ViewBag.IsRequetorReOpen = ViewBag.processApprove == false && model.Status == StatusTransactionName.Close;
                model.Year = Convert.ToDateTime(model.StartDate).Year;
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.Status = StatusTransactionName.Draft;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<FinancialYear>>(url) ?? new List<FinancialYear>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPut]
        public async Task<IActionResult> Approve(FinancialYear model)
        {
            model.StatusApproval = StatusTransaction.Approved;
            var response = await client.PutApiResponse<FinancialYear>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(FinancialYear model)
        {
            model.StatusApproval = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<FinancialYear>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(FinancialYear model)
        {
            model.IsClose = true;
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model);
        }

        [HttpPut]
        public async Task<IActionResult> Reopen(FinancialYear model)
        {
            model.IsClose = false;
            model.StatusApproval = StatusTransaction.WaitingApproval;
            return await Process(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(FinancialYear model)
        {
            model.IsClose = false;
            model.StatusApproval = StatusTransaction.Draft;
            return await Process(model);
        }

        private async Task<IActionResult> Process(FinancialYear model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<FinancialYear>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<FinancialYear>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete(FinancialYear model)
        //{
        //    var response = await client.DeleteApiResponse<FinancialYear>($"{url}/{model.Id}");
        //    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        //}


        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var model = await client.Get<FinancialYear>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> MaxDate(string legalEntityId)
        {
            var date = await client.Get<DateTime>($"{url}/MaxDate/{legalEntityId}");
            return Json(date.ToString("yyyy-MM-dd"), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [HttpGet]
        public IActionResult GetDate(string year)
        {
            DateTime startDate = Convert.ToDateTime($"{year}/1/1");
            DateTime endDate = startDate.AddYears(1).AddDays(-1);

            return Json(new { startDate, endDate }, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetDefaultYearOpeningBalance(string legalEntityId)
        {
            var year = await client.Get<int?>($"{url}/GetDefaultYearOpeningBalance/{legalEntityId}");
            return Json(year, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}