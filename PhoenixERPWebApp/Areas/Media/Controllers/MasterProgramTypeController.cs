using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media.MasterData;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MasterProgramTypeController : Controller
    {
        string url = ApiUrl.ProgramType;
        private readonly ApiClientFactory client;

        public MasterProgramTypeController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<ProgramType>>(url) ?? new List<ProgramType>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new ProgramType();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<ProgramType>($"{url}/{Id}");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ProgramType model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<ProgramType>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<ProgramType>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ProgramType model) => Json(await client.DeleteApiResponse<ProgramType>($"{url}/{model.Id}"));
    }
}