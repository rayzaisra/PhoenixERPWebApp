using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using Kendo.Mvc.UI;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phoenix.WebExtension.Extentions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class ReconcileController : Controller
    {
        string url = ApiUrl.ReconcileUrl;
        private readonly ApiClientFactory client;
        public ReconcileController(ApiClientFactory _client)
        {
            this.client = _client;
        }

        public IActionResult Index()
        {
            var search = new ReconcileSearch() { Type = "CV" };
            return View(search);
        }

        public async Task<IActionResult> Detail(string Id, string ReferenceId)
        {
            var model = new Reconcile() { ReferenceId = ReferenceId };
            if (!string.IsNullOrEmpty(Id)) { model = await client.Get<Reconcile>($"{url}/{Id}"); }
            return PartialView("_Reconcile", model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, ReconcileSearch search)
        {
            var model = await client.PostApiResponse<List<ReconcileList>>($"{url}/GetList", search);
            var List = new List<ReconcileList>();
            if (model.Data != null) List = model.Data as List<ReconcileList>;
            var data = List.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(Reconcile model)
        {
            var response = await client.PostApiResponse<Reconcile>(url, model);
            return Json(new { success = response.Success, message = response.Message, refreshGrid = "grid", hideModalPopUp = "modalReconcile" });
        }
    }
}