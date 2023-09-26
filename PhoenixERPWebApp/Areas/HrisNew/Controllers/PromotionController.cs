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
    public class PromotionController : Controller
    {
        string url = ApiUrl.PromotionUrl;
        private readonly ApiClientFactory client;

        public PromotionController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexApproval()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id, bool isApprove)
        {
            var model = new Promotion();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<Promotion>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = requestor;
                model.Requester = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                model.SubmitUser = false;
                model.infoEmployee = new InfoEmployee();
                ViewBag.isRequestor = isApprove == false;
            }

            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.IsApprove = isApprove;
            ViewBag.isHcAdmin = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsHCAdmin");
            if (model.IncrementPercentage == null) model.IncrementPercentage = 0;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Promotion>>(url) ?? new List<Promotion>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Promotion>>($"{url}/approvalList") ?? new List<Promotion>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Promotion model)
        {
            //model.Status = StatusTransaction.Process;
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Promotion>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Promotion model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Promotion>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Promotion model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitHC(Promotion model)
        {
            model.Status = StatusTransaction.Draft;
            model.SubmitUser = true;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Promotion model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(Promotion model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Promotion>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Promotion>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Promotion });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Promotion model) => Json(await client.DeleteApiResponse<Promotion>($"{url}/{model.Id}"));

        [HttpGet]
        public async Task<IActionResult> GetAdditionalList(string employeeId, string promotionId)
        {
            var model = await client.Get<List<PromotionSubcomponentAdditional>>($"{url}/additional/{employeeId}/{promotionId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetDeductionList(string employeeId, string promotionId)
        {
            var model = await client.Get<List<PromotionSubcomponentDeduction>>($"{url}/deducation/{employeeId}/{promotionId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}