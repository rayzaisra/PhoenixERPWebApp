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
    public class PDRObjectiveTargetController : Controller
    {
        string url = ApiUrl.PdrObjectiveTargetUrl;
        private readonly ApiClientFactory client;

        public PDRObjectiveTargetController(ApiClientFactory client)
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
            var model = new PdrObjectiveTarget();
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<PdrObjectiveTarget>($"{url}/GetById/{Id}");
                if (isApprove == true)
                    ViewBag.Header = "Approve";
                else
                    ViewBag.Header = "Update";
            }
            else
            {
                ViewBag.Header = "Create";
                model.infoEmployee = new InfoEmployee();
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<PdrObjectiveTarget>>(url) ?? new List<PdrObjectiveTarget>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadPdrObjectiveTargetDetail([DataSourceRequest] DataSourceRequest request, string PdrObjectiveTargetId)
        {
            if (PdrObjectiveTargetId == null) PdrObjectiveTargetId = "0";
            var model = await client.Get<List<PdrObjectiveTargetDetail>>($"{url}/gettPdrObjectiveTargetDetailList/{PdrObjectiveTargetId}") ?? new List<PdrObjectiveTargetDetail>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(PdrObjectiveTarget model)
        {
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(PdrObjectiveTarget model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<PdrObjectiveTarget>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<PdrObjectiveTarget>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as PdrObjectiveTarget });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PdrObjectiveTarget model) => Json(await client.DeleteApiResponse<PdrObjectiveTarget>($"{url}/{model.Id}"));

        //PdrObjectiveTargetDetail
        [AcceptVerbs("Post")]
        public ActionResult CreatePdrObjectiveTargetDetail([DataSourceRequest] DataSourceRequest request, PdrObjectiveTargetDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult UpdatePdrObjectiveTargetDetail([DataSourceRequest] DataSourceRequest request, PdrObjectiveTargetDetail model)
        {
            if (model != null && ModelState.IsValid)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult DestroyPdrObjectiveTargetDetail([DataSourceRequest] DataSourceRequest request, PdrObjectiveTargetDetail model)
        {
            if (model != null)
            {
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}