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
    public class SalaryPaymentRequestController : Controller
    {
        string url = ApiUrl.SalaryPaymentRequestUrl;
        private readonly ApiClientFactory client;

        public SalaryPaymentRequestController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new SalaryPaymentRequest();
            ViewBag.processApprove = false;

            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<SalaryPaymentRequest>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == StatusTransactionName.Draft || model.Status == StatusTransactionName.Rejected);
                model.RequestDate = model.CreatedOn.Value.ToString("dd MMMM yyyy");
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.Status = StatusTransactionName.Draft;
                model.CurrencyId = CurrencyIDR.CurrencyId;
                model.CreatedOn = DateTime.Now;
                model.RequestDate = DateTime.Now.ToString("dd MMMM yyyy");
                model.RequestBy = requestor.EmployeeName;
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;

            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<SalaryPaymentRequest>>(url) ?? new List<SalaryPaymentRequest>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string id, string legalEntityId, DateTime? salaryPaymentDate)
        {
            int month = 0, year = 0;
            if (salaryPaymentDate != null)
            {
                month = salaryPaymentDate.Value.Month;
                year = salaryPaymentDate.Value.Year;
            }

            var model = await client.Get<List<SalaryPaymentRequestDetail>>($"{url}/Details/{id ?? "0"}/{legalEntityId ?? "0"}/{month}/{year}") ?? new List<SalaryPaymentRequestDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<Journal>>($"{url}/GetDataJournal/{id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(SalaryPaymentRequest model)
        {
            model.Status = StatusTransactionName.Draft;
            return await Process(model, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SalaryPaymentRequest model)
        {
            model.Status = StatusTransactionName.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(SalaryPaymentRequest model)
        {
            var response = await client.PostApiResponse<SalaryPaymentRequest>($"{url}/CreateJournal", model);
            var data = response.Data as SalaryPaymentRequest;
            //clear remarks log
            var setHiddenValue = new[] { new { id = "TotalAmount", value = data.TotalAmount.ToString() } };
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridjournal;gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
        }

        [HttpPut]
        public async Task<IActionResult> Approve(SalaryPaymentRequest model)
        {
            var response = await client.PutApiResponse<SalaryPaymentRequest>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(SalaryPaymentRequest model)
        {
            var response = await client.PutApiResponse<SalaryPaymentRequest>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        private async Task<IActionResult> Process(SalaryPaymentRequest model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<SalaryPaymentRequest>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<SalaryPaymentRequest>(url, model);
            }

            if (isDraft)
            {
                var data = response.Data as SalaryPaymentRequest;
                //clear remarks log
                var setHiddenValue = new[] { new { id = "TotalAmount", value = model.TotalAmount.ToString() }, new { id = "LogDescription", value = "" } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridstatuslogHistoryList", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(SalaryPaymentRequest model) => Json(await client.DeleteApiResponse<SalaryPaymentRequest>($"{url}/{model.Id}"));

        public async Task<IActionResult> GetPayrollId(string legalEntityId, DateTime? salaryPaymentDate)
        {
            int month = 0, year = 0;
            if (salaryPaymentDate != null)
            {
                month = salaryPaymentDate.Value.Month;
                year = salaryPaymentDate.Value.Year;
            }

            var model = await client.Get<string>($"{url}/GetPayrollId/{legalEntityId ?? "0"}/{month}/{year}");
            return Json(model);
        }
    }
}