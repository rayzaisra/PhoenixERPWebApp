using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class PDRApprailsalController : Controller
    {
        string url = ApiUrl.PdrApprailsalUrl;
        private readonly ApiClientFactory client;

        public PDRApprailsalController(ApiClientFactory client)
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
            var model = new PdrApprailsalM1Header();
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<PdrApprailsalM1Header>($"{url}/GetById/{Id}");
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = isApprove == false && (model.Status == StatusTransaction.Draft || model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                model.infoEmployee = new InfoEmployee();
                ViewBag.isRequestor = isApprove == false;
                model.Status = StatusTransaction.Draft;
            }
            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval);
            ViewBag.m1Up = model.infoEmployee.Grade == "M1" || model.infoEmployee.Grade == "E1" || model.infoEmployee.Grade == "E2" || model.infoEmployee.Grade == "E3" || model.infoEmployee.Grade == "BOD";
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PdrApprailsalM1Header>>(url) ?? new List<PdrApprailsalM1Header>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PdrApprailsalM1Header>>($"{url}/approvalList") ?? new List<PdrApprailsalM1Header>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadPdrApprailsalM1([DataSourceRequest] DataSourceRequest request, string PdrApprailsalM1HeaderId, string employeeId, DateTime? startReview)
        {
            if (PdrApprailsalM1HeaderId == null) PdrApprailsalM1HeaderId = "0";
            if (employeeId == null) employeeId = "0";
            if (startReview == null) startReview = DateTime.Now;
            var model = await client.Get<List<PdrApprailsalM1>>($"{url}/getPdrApprailsalM1List/{PdrApprailsalM1HeaderId}/{employeeId}/{Convert.ToDateTime(startReview).ToString("yyyy-MM-dd")}") ?? new List<PdrApprailsalM1>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PdrApprailsalM1Header model)
        {
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<PdrApprailsalM1Header>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(PdrApprailsalM1Header model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<PdrApprailsalM1Header>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PdrApprailsalM1Header model)
        {
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(PdrApprailsalM1Header model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(PdrApprailsalM1Header model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PdrApprailsalM1Header>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<PdrApprailsalM1Header>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as PdrApprailsalM1Header });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PdrApprailsalM1Header model) => Json(await client.DeleteApiResponse<PdrApprailsalM1Header>($"{url}/{model.Id}"));

        //PdrApprailsalM1
        [AcceptVerbs("Post")]
        public ActionResult CreatePdrApprailsalM1([DataSourceRequest] DataSourceRequest request, PdrApprailsalM1 model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdatePdrApprailsalM1([DataSourceRequest] DataSourceRequest request, PdrApprailsalM1 model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyPdrApprailsalM1([DataSourceRequest] DataSourceRequest request, PdrApprailsalM1 model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}