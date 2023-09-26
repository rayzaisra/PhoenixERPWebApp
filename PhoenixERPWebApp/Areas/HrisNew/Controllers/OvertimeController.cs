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
    public class OvertimeController : Controller
    {
        string url = ApiUrl.OvertimeUrl;
        private readonly ApiClientFactory client;

        public OvertimeController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            //ViewBag.isRequest = await client.Get<bool>($"{url}/IsRequest");
            return View();
        }

        public IActionResult IndexApproval()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public async Task<IActionResult> ReportDetail(string Id, string year)
        {
            var model = new OvertimeReportDetail();
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<OvertimeReportDetail>($"{url}/GetDetailOvertimeReport/{Id}/{year}");
            }
            return View(model);
        }

        public async Task<IActionResult> AddEdit(string Id, bool isApprove)
        {
            var model = new Overtime();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<Overtime>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = requestor;
                model.EmployeeBasicInfoId = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                ViewBag.isRequestor = isApprove == false;
            }
            if (!string.IsNullOrEmpty(model.ReasonOvertime))
            {
                model.ReasonOvertime = HttpUtility.HtmlDecode(model.ReasonOvertime);
            }
           
            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Overtime>>(url) ?? new List<Overtime>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Overtime>>($"{url}/approvalList") ?? new List<Overtime>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadOvertimeReport([DataSourceRequest] DataSourceRequest request, string year)
        {
            var model = await client.Get<List<OvertimeReport>>($"{url}/overtimeReport/{year}") ?? new List<OvertimeReport>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadOvertimeReportDetail([DataSourceRequest] DataSourceRequest request, string Id, string year)
        {
            var model = await client.Get<List<OvertimeReportBody>>($"{url}/GetDetailOvertimeReportBody/{Id}/{year}") ?? new List<OvertimeReportBody>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Overtime model)
        {
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Overtime>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Overtime model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Overtime>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Overtime model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Overtime model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        [HttpPost]
        private async Task<IActionResult> Process(Overtime model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Overtime>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Overtime>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Overtime });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(Overtime model)
        {
            var response = await client.DeleteApiResponse<Overtime>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}