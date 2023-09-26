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
    public class EmployeeLoanCoopController : Controller
    {
        string url = ApiUrl.EmployeeLoanUrl;
        private readonly ApiClientFactory client;
		private readonly string CoopLoanCategoryId = "83a48346-68ec-409a-ad8b-0bed39ce0828";

		public EmployeeLoanCoopController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new EmployeeLoan();
            ViewBag.Header = "Create";
            ViewBag.processApprove = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (ViewBag.processApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<EmployeeLoan>($"{url}/{Id}");
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
            }

			LoanCategory modelloancategory = null;
			modelloancategory = await client.Get<LoanCategory>($"{ApiUrl.LoanCategoryUrl}/{CoopLoanCategoryId}");
			if (modelloancategory == null)
				modelloancategory = new LoanCategory();

			model.InterestMonth = modelloancategory.InterestMonth;
            model.LoanCategoryId = CoopLoanCategoryId;
			return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
			var model = await client.Get<List<EmployeeLoan>>($"{url}/GetLisCoop") ?? new List<EmployeeLoan>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(EmployeeLoan model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            model.Status = EmployeeLoanStatus.Open;
            model.IsCorporateLoan = false;

            //return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            return await Process(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(EmployeeLoan model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            model.Status = EmployeeLoanStatus.WAITING_APPROVAL;
            model.IsCorporateLoan = false;
            //return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            return await Process(model);
        }

        [HttpPut]
        public async Task<IActionResult> Approve(EmployeeLoan model)
        {
            model.StatusApproval = EmployeeLoanStatus.Approved;
            model.Status = EmployeeLoanStatus.Approved;
            model.IsCorporateLoan = false;
            var response = await client.PutApiResponse<EmployeeLoan>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPut]
        public async Task<IActionResult> Reject(EmployeeLoan model)
        {
            model.StatusApproval = EmployeeLoanStatus.Reject;
            model.Status = EmployeeLoanStatus.Reject;
            model.IsCorporateLoan = false;
            var response = await client.PutApiResponse<EmployeeLoan>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Process(EmployeeLoan model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<EmployeeLoan>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<EmployeeLoan>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(EmployeeLoan model) => Json(await client.DeleteApiResponse<EmployeeLoan>($"{url}/{model.Id}"));

        [HttpGet]
        public async Task<IActionResult> GetLoanCategoryById(string loanCategoryId)
        {
            var model = await client.Get<LoanCategory>($"{ApiUrl.LoanCategoryUrl}/{loanCategoryId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadLoanPaid(string id)
        {
            var model = await client.Get<List<EmployeeLoanPaid>>($"{url}/getLoanPaidList/{id}") ?? new List<EmployeeLoanPaid>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

		[HttpPost]
		public async Task<IActionResult> SaveDtlPaidEarly(EmployeeLoan model, List<EmployeeLoanPaid> dtl)
		{
			var response = new Phoenix.WebExtension.RestApi.ApiResponse();
			if (dtl.Count > 0) {
				string urls = $"{url}/AddEditLoanPaidAsync";
				response = await client.PostApiResponse<EmployeeLoan>(urls, model);
			}

			return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
		}
	}
}