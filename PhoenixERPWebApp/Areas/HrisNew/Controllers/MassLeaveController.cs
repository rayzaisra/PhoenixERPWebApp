using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    public class MassLeaveController : Controller
    {
        string url = ApiUrl.MassLeaveUrl;
        private readonly ApiClientFactory client;

        public MassLeaveController(ApiClientFactory client)
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
            var model = new MassLeave();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<MassLeave>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = requestor;
                model.RequestBy = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                model.LeaveFrom = DateTime.Now.AddDays(1);
                model.LeaveTo = DateTime.Now.AddDays(1);
                model.TotalDays = 1;
                model.Status = StatusTransaction.Draft;
                model.StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft);
                ViewBag.isRequestor = isApprove == false;
            }

            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MassLeave>>(url) ?? new List<MassLeave>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MassLeave>>($"{url}/approvalList") ?? new List<MassLeave>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAssisted([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<MassLeaveDetail>>($"{url}/assignList/{id}") ?? new List<MassLeaveDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(MassLeave model)
        {
            model.Reason = HttpUtility.HtmlDecode(model.Reason);
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<MassLeave>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(MassLeave model)
        {
            model.Reason = HttpUtility.HtmlDecode(model.Reason);
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<MassLeave>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MassLeave model, List<MassLeaveDetail> MassLeaveDetails)
        {
            model.Status = StatusTransaction.WaitingApproval;
            model.MassLeaveDetails = MassLeaveDetails;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(MassLeave model, List<MassLeaveDetail> MassLeaveDetails)
        {
            model.Status = StatusTransaction.Draft;
            model.MassLeaveDetails = MassLeaveDetails;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(MassLeave model, bool isDraft)
        {
            model.Reason = HttpUtility.HtmlDecode(model.Reason);
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MassLeave>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MassLeave>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as MassLeave , url = Url.Action(nameof(Index)) });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MassLeave model) => Json(await client.DeleteApiResponse<MassLeave>($"{url}/{model.Id}"));
    }
}