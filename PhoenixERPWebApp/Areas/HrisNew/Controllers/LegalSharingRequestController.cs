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
    public class LegalSharingRequestController : Controller
    {
        string url = ApiUrl.LegalSharingRequestUrl;
        private readonly ApiClientFactory client;

        public LegalSharingRequestController(ApiClientFactory client)
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
            var model = new LegalSharingRequest();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<LegalSharingRequest>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = isApprove == false;
            }

            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<LegalSharingRequest>>(url) ?? new List<LegalSharingRequest>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<LegalSharingRequest>>($"{url}/approvalList") ?? new List<LegalSharingRequest>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(LegalSharingRequest model)
        {
            //model.Status = StatusTransaction.Process;
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<LegalSharingRequest>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(LegalSharingRequest model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<LegalSharingRequest>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LegalSharingRequest model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(LegalSharingRequest model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(LegalSharingRequest model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<LegalSharingRequest>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<LegalSharingRequest>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as LegalSharingRequest });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(LegalSharingRequest model) => Json(await client.DeleteApiResponse<LegalSharingRequest>($"{url}/{model.Id}"));
    }
}