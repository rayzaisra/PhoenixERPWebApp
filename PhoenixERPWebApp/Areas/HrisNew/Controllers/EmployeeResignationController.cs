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
    public class EmployeeResignationController : Controller
    {
        string url = ApiUrl.ResignationUrl;
        private readonly ApiClientFactory client;

        public EmployeeResignationController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.isHcAdmin = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsHCAdmin");
            ViewBag.isRequest = true;
            if (ViewBag.isHcAdmin)
            {
                ViewBag.isRequest = false;
            }
            else
            {
                ViewBag.isRequest = await client.Get<bool>($"{url}/IsRequest");
            }
            
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
                if (model == null)
                {
                    model = new Termination();
                    InfoEmployee ie = new InfoEmployee();
                    model.requestEmployee = ie;
                    model.infoEmployee = ie;
                }
                model.SearchEmployeeId = model.infoEmployee.Id;
                model.SearchEmployee = model.infoEmployee.EmployeeName;
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft);
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                //|| model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = requestor;
                model.Requestor = requestor.EmployeeId;
                model.EmployeeBasicInfoId = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                model.TrainingBonding = 0;
                model.Penalty = 0;

                ViewBag.isRequestor = ((await client.Get<bool>($"{url}/IsRequest")) == false) && isApprove == false;
                //ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft);
            }
            ViewBag.isHcAdmin = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsHCAdmin");
            if (ViewBag.isHcAdmin)
            {
                ViewBag.isRequestor = true;
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
        public async Task<IActionResult> Approve(Termination model)
        {
            model.Status = StatusTransaction.Approved;
            model.TerminationType = TerminationType.Resignation;
            var response = await client.PutApiResponse<Termination>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Termination model)
        {
            model.Status = StatusTransaction.Rejected;
            model.TerminationType = TerminationType.Resignation;
            var response = await client.PutApiResponse<Termination>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Termination model, IFormFile file)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, file, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Termination model, IFormFile file)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, file, true);
        }

        private async Task<IActionResult> Process(Termination model, IFormFile file, bool isDraft)
        {
            model.TerminationType = TerminationType.Resignation;
            model.requestEmployee = null;
            if (!string.IsNullOrEmpty(model.SearchEmployee))
            {
                model.EmployeeBasicInfoId = model.SearchEmployeeId;
            }
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id) )
            {
                response = await client.PutApiResponse<Termination>(url, model, file);
            }
            else
            {
                response = await client.PostApiResponse<Termination>(url, model, file);
            }

            //if (isDraft)
            //{
            //    return Json(new { success = response.Success, message = response.Message, data = response.Data as Termination });
            //}
            //else
            //{
            //    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            //}
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(Termination model)
        {
            var response = await client.DeleteApiResponse<Termination>($"{url}/{model.Id}");
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpGet]
        public async Task<IActionResult> GetDataResignationByEmployeeId(string id)
        {
            var model = await client.Get<Termination>($"{url}/GetDataResignationByEmployeeId/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}