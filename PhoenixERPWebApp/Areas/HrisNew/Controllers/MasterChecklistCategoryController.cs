using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.HrisNew.MasterData.MasterChecklistCategory;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class MasterChecklistCategoryController : Controller
    {
        string url = ApiUrl.MasterChecklistCategory;
        private readonly ApiClientFactory client;
        public MasterChecklistCategoryController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new MasterChecklistCategory();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<MasterChecklistCategory>($"{url}/{Id}");
                model.Is_Default = model.IsDefault ?? false;
                model.Is_Locked = model.IsLocked ?? false;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MasterChecklistCategory>>(url) ?? new List<MasterChecklistCategory>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MasterChecklistCategory model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<MasterChecklistCategory>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<MasterChecklistCategory>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MasterChecklistCategory model) => Json(await client.DeleteApiResponse<MasterChecklistCategory>($"{url}/{model.Id}"));
    }
}