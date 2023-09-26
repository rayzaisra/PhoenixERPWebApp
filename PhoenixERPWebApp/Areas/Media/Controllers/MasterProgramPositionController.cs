using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Media.MasterData;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MasterProgramPositionController : Controller
    {
        string url = ApiUrl.ProgramPosition;
        private readonly ApiClientFactory client;

        public MasterProgramPositionController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<ProgramPosition>>(url) ?? new List<ProgramPosition>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new ProgramPosition();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<ProgramPosition>($"{url}/{Id}");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ProgramPosition model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<ProgramPosition>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<ProgramPosition>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ProgramPosition model) => Json(await client.DeleteApiResponse<ProgramPosition>($"{url}/{model.Id}"));
    }
}