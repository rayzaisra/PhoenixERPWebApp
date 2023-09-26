using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Um;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class EmailTemplateController : BaseController
    {
        string url = ApiUrl.EmailTemplateUrl;
        private readonly ApiClientFactory client;
        private string _uniqueName = "MSTRTEMPEMAIL";

        public EmailTemplateController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new TmEmailTemplate();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<TmEmailTemplate>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TmEmailTemplate>>(url) ?? new List<TmEmailTemplate>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(TmEmailTemplate model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                model.IsHtml = model.IsHtmlVal;
                response = await client.PutApiResponse<TmEmailTemplate>(url, model);
            }
            else
            {
                model.IsHtml = model.IsHtmlVal;
                response = await client.PostApiResponse<TmEmailTemplate>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(TmEmailTemplate model) => Json(await client.DeleteApiResponse<TmEmailTemplate>($"{url}/{model.Id}"));
    }
}