using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance;
using PhoenixERPWebApp.Models.Global;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class PettyCashController : BaseController
    {
        private readonly ApiClientFactory client;
        string url = ApiUrl.PettyCashUrl;
        string url_dropdown = ApiUrl.financedropdownUrl;

        public PettyCashController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.IsHolder = await client.Get<bool>($"{url}/IsHolder");
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PettyCash>>(url) ?? new List<PettyCash>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string Id)
        {
            ViewBag.processApprove = false;
            var model = new PettyCash();
            if (string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Create";
                ViewBag.isRequestor = ViewBag.processApprove == false;
                model.Status = PettyCashStatus.Open;
                model.SettlementDate = DateTime.Now;
                model.CreatedOn = DateTime.Now;
            }
            else
            {
                model = await client.Get<PettyCash>($"{url}/GetById/{Id}");
                if (ViewBag.processApprove == true) ViewBag.Header = "Approval"; else ViewBag.Header = "Update";
                ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                ViewBag.isRequestor = ViewBag.processApprove == false && (model.Status == PettyCashStatus.Open || model.Status == PettyCashStatus.REVISE);
            }
            return View(model);
        }

        public async Task<IActionResult> GetPettyCashDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var data = await client.Get<List<PettyCashDetail>>($"{url}/GetDetails/{Id}") ?? new List<PettyCashDetail>();
            var model = data.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<Journal>>($"{url}/JournalList/{Id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetReffPettyCash(string Id)
        {
            var model = await client.Get<List<MasterPettyCash>>($"{url}/GetMasterPettyCash/{Id}") ?? new List<MasterPettyCash>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        public async Task<JsonResult> GetInfoMasterPetty(string id)
        {
            var model = await client.Get<PettyCash>($"{url}/GetMasterPettyCashById/{id}") ?? new PettyCash();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult CreateDetail([DataSourceRequest] DataSourceRequest request, PettyCashDetail model)
        {
            if (model != null && ModelState.IsValid) { model.Id = Guid.NewGuid().ToString(); }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdateDetail([DataSourceRequest] DataSourceRequest request, PettyCashDetail model)
        {
            if (model != null && ModelState.IsValid) { }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyDetail([DataSourceRequest] DataSourceRequest request, PettyCashDetail model)
        {
            if (model != null) { }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PettyCash model, IFormFile fileEvidence)
        {
            model.Status = PettyCashStatus.WAITING_APPROVAL;
            return await Process(model, fileEvidence, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(PettyCash model, IFormFile fileEvidence)
        {
            model.Status = PettyCashStatus.Open;
            return await Process(model, fileEvidence, true);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournal(PettyCash model, IFormFile fileEvidence)
        {
            var response = await client.PostApiResponse<PettyCash>($"{url}/CreateJournal", model, fileEvidence);
            var data = response.Data as PettyCash;
            var setHiddenValue = new[] { new { id = "CreateDate", value = Convert.ToDateTime(data.CreatedOn).ToString("dd MMMM yyyy") } };
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "gridJournal", setHiddenValue = setHiddenValue, data = data });
        }

        private async Task<IActionResult> Process(PettyCash model, IFormFile fileEvidence, bool isDraft)
        {
            var response = new ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PettyCash>(url, model, fileEvidence);
            }
            else
            {
                response = await client.PostApiResponse<PettyCash>(url, model, fileEvidence);
            }

            if (isDraft)
            {
                var data = response.Data as PettyCash;
                var setHiddenValue = new[] { new { id = "CreateDate", value = Convert.ToDateTime(data.CreatedOn).ToString("dd MMMM yyyy") } };
                return Json(new { success = response.Success, message = response.Message, refreshGrid = "grid", setHiddenValue = setHiddenValue, data = data });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reject(PettyCash model)
        {
            var response = await client.PutApiResponse<PettyCash>($"{url}/Reject", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PettyCash model)
        {
            model.Status = PettyCashStatus.Approved;
            var response = await client.PutApiResponse<PettyCash>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        public async Task<IActionResult> GetInfoPceByJobId([DataSourceRequest] DataSourceRequest request, string jobId)
        {
            var model = await client.Get<JobDetail>($"{url}/GetInfoPceByJobId/{jobId}") ?? new JobDetail();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<ActionResult> GetPcaTasks(string pcaId)
        {
            var model = await client.Get<List<SelectListItem>>($"{url}/GetPcaTasks/{pcaId}") ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}