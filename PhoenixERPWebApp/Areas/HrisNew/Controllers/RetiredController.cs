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
    public class RetiredController : Controller
    {
        string url = ApiUrl.RetiredUrl;
        private readonly ApiClientFactory client;

        public RetiredController(ApiClientFactory client)
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
            var model = new Termination();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<Termination>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var terminationType = await client.Get<TmTerminationType>($"{ApiUrl.TmTerminationTypeUrl}/{TerminationType.Retired}");
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = requestor;
                model.Requestor = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                model.TerminationType = terminationType.Id;
                model.TerminationTypeName = terminationType.TypeName;
                ViewBag.isRequestor = isApprove == false;
                model.infoEmployee = new InfoEmployee();
            }

            model.Reason = HttpUtility.HtmlDecode(model.Reason);
            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Termination>>(url) ?? new List<Termination>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Termination>>($"{url}/approvalList") ?? new List<Termination>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Termination model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Termination model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Termination model)
        {
            model.TerminationType = TerminationType.Retired;
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Termination>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Termination model)
        {
            model.TerminationType = TerminationType.Retired;
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Termination>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        private async Task<IActionResult> Process(Termination model, bool isDraft)
        {
            model.TerminationType = TerminationType.Retired;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Termination>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Termination>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Termination });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Termination model) => Json(await client.DeleteApiResponse<Termination>($"{url}/{model.Id}"));
    }
}