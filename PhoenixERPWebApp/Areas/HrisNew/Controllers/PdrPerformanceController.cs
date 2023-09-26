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
    public class PdrPerformanceController : Controller
    {
        string url = ApiUrl.PdrPerformanceUrl;
        private readonly ApiClientFactory client;
        private readonly IMemoryCache cache;

        public PdrPerformanceController(ApiClientFactory client, IMemoryCache cache)
        {
            this.client = client;
            this.cache = cache;
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
            var model = await client.Get<PdrEmployee>($"{url}/GetById/{Id}");
            var isCurrentApproval = false;
            if (!string.IsNullOrEmpty(Id))
            {
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
                //var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                //model.requestEmployee = requestor;
                //model.Requester = requestor.EmployeeId;
                //model.RequestDate = DateTime.Now;
                model.infoEmployee = new InfoEmployee();
                ViewBag.isRequestor = isApprove == false;
                model.Status = StatusTransaction.Draft;
            }

            if (model.PdrRecomendation == null) model.PdrRecomendation = new PdrRecomendation();
            if (model.PdrDetailM2belows == null) model.PdrDetailM2belows = new List<PdrDetailM2below>();
            if (model.PdrOverallScores == null) model.PdrOverallScores = new List<PdrOverallScore>();

            ViewBag.processApprove = isApprove && isCurrentApproval;//(model.Status == StatusTransaction.WaitingApproval || model.Status == StatusTransaction.CurrentApproval); 
            model.Notes = HttpUtility.HtmlDecode(model.Notes);
            model.IsApprove = isApprove;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PdrEmployee>>(url) ?? new List<PdrEmployee>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadApproval([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PdrEmployee>>($"{url}/approvalList") ?? new List<PdrEmployee>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadPdrDevelopmentPlan([DataSourceRequest] DataSourceRequest request, string pdrEmployeeId)
        {
            if (pdrEmployeeId == null) pdrEmployeeId = "0";
            var model = await client.Get<List<PdrDevelopmentPlan>>($"{url}/getPdrDevelopmentPlanList/{pdrEmployeeId}") ?? new List<PdrDevelopmentPlan>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PdrEmployee model)
        {
            model.Status = StatusTransaction.Approved;
            var response = await client.PutApiResponse<PdrEmployee>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(PdrEmployee model)
        {
            model.Status = StatusTransaction.Rejected;
            var response = await client.PutApiResponse<PdrEmployee>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PdrEmployee model)
        {
            model.DateSubmit = DateTime.Now;
            model.DateAppraisal = DateTime.Now;
            model.Year = DateTime.Now.Year;
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(PdrEmployee model)
        {
            model.Status = StatusTransaction.Draft;
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(PdrEmployee model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PdrEmployee>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<PdrEmployee>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as PdrEmployee });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PdrEmployee model) => Json(await client.DeleteApiResponse<PdrEmployee>($"{url}/{model.Id}"));

        //PdrDevelopmentPlan
        [AcceptVerbs("Post")]
        public ActionResult CreatePdrDevelopmentPlan([DataSourceRequest] DataSourceRequest request, PdrDevelopmentPlan model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdatePdrDevelopmentPlan([DataSourceRequest] DataSourceRequest request, PdrDevelopmentPlan model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyPdrDevelopmentPlan([DataSourceRequest] DataSourceRequest request, PdrDevelopmentPlan model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}