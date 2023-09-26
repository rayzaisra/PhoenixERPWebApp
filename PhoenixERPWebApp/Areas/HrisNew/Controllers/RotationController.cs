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
    public class RotationController : Controller
    {
        string url = ApiUrl.RotationUrl;
        private readonly ApiClientFactory client;

        public RotationController(ApiClientFactory client)
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
            var model = new Rotation();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                model = await client.Get<Rotation>($"{url}/{Id}");               
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft); //|| model.Status == StatusTransaction.Rejected);

                if (model.IncrementPercentage == null) model.IncrementPercentage = 0;
                if (model.NextSalary == null) model.NextSalary = model.CurrentSalary;
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
            model.Notes = HttpUtility.HtmlDecode(model.Notes);
            model.IsApprove = isApprove;
            ViewBag.isHcAdmin = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsHCAdmin");
            if (model.IncrementPercentage == null) model.IncrementPercentage = 0;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Rotation>>(url) ?? new List<Rotation>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Rotation>>($"{url}/approvalList") ?? new List<Rotation>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Rotation model)
        {
            //model.Status = StatusTransaction.Process;
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<Rotation>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Rotation model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<Rotation>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Rotation model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitHC(Rotation model)
        {
            model.Status = StatusTransaction.Draft;
            model.SubmitUser = true;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Rotation model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(Rotation model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Rotation>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Rotation>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Rotation });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Rotation model) => Json(await client.DeleteApiResponse<Rotation>($"{url}/{model.Id}"));

        [HttpGet]
        public async Task<IActionResult> GetEmployeeSalaryinfoById(string employeeId)
        {
            var model = await client.Get<Rotation>($"{url}/getEmployeeSalaryinfoById/{employeeId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAdditionalList(string employeeId, string rotationId)
        {
            var model = await client.Get<List<RotationSubcomponentAdditional>>($"{url}/additional/{employeeId}/{rotationId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<IActionResult> GetDeductionList(string employeeId, string rotationId)
        {
            var model = await client.Get<List<RotationSubcomponentDeduction>>($"{url}/deducation/{employeeId}/{rotationId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}