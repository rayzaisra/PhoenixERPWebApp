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
    public class LeaveController : Controller
    {
        string url = ApiUrl.LeaveUrl;
        string urlHoliday = ApiUrl.HolidayUrl;
        string urlLeaveRequestType = ApiUrl.LeaveRequestTypeUrl;
        private readonly ApiClientFactory client;

        public LeaveController(ApiClientFactory client)
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
            var model = new Leave();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<Leave>($"{url}/{Id}");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var isEmployeeLeave = await client.Get<bool>($"{url}/isEmployeeLeave/{requestor.EmployeeId}");
                model.requestEmployee = requestor;
                model.EmployeeBasicInfoId = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
                model.TotalDays = 1;
                model.UseCurrentYear = true;
                model.Status = StatusTransaction.Draft;
                model.StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft);
                ViewBag.isRequestor = isApprove == false;

                var dateDiffJoin = DateTime.Now.Subtract(requestor.DateEmployed.Value).Days / (365.25 / 12);
                if (requestor.EmployementStatusId != "1066372149285097472")
                {
                    if (dateDiffJoin < 6)
                    {
                        ViewBag.isRequestor = false;
                    }
                }

                if (isEmployeeLeave)
                {
                    ViewBag.isRequestor = false;
                }


            }

            model.Reason = HttpUtility.HtmlDecode(model.Reason);
            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Leave>>(url) ?? new List<Leave>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Leave>>($"{url}/approvalList") ?? new List<Leave>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadAssisted([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<InfoEmployee>>($"{url}/assignList/{id}") ?? new List<InfoEmployee>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadLeaveBalance([DataSourceRequest] DataSourceRequest request, string employeeId,string leaveRequestTypeId)
        {

            var model = await client.Get<TableLeaveBalance>($"{ApiUrl.LeaveBalanceUrl}/getEmployeeLeaveBalaceInfoList/{(employeeId ?? "0")}/{leaveRequestTypeId}") ?? new TableLeaveBalance();
            var resultTable = model.ListTableLeaveBalance;
            var data = resultTable.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Leave model)
        {
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Leave>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Leave model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Leave>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Leave model, IFormFile file)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false, file);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Leave model,IFormFile file)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true, file);
        }

        private async Task<IActionResult> Process(Leave model, bool isDraft, IFormFile file)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Leave>(url, model, file);
            }
            else
            {
                response = await client.PostApiResponse<Leave>(url, model, file);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Leave });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Leave model) => Json(await client.DeleteApiResponse<Leave>($"{url}/{model.Id}"));

        [HttpGet]
        public async Task<IActionResult> GetLeaveBalanceInfoById(string employeeId, DateTime endDate)
        {
            var model = await client.Get<Leave>($"{url}/getLeaveBalanceInfoById/{employeeId}/{endDate.ToString("yyyy-MM-dd")}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> CalculateWorkingDays(DateTime startDate, DateTime endDate)
        {
            var result = await WorkingDays.GetWorkingDays(client, startDate, endDate);
            return Json(result);
        }

        public async Task<IActionResult> GetHolidays()
        {
            var model = await client.Get<List<Holiday>>(urlHoliday) ?? new List<Holiday>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveRequestType(string Id)
        {
            var model = await client.Get<LeaveRequestType>($"{urlLeaveRequestType}/{Id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        private class Holiday
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }
    }
}