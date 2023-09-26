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
using PhoenixERPWebApp.Models.Media;

namespace PhoenixERPWebApp.Areas.Media.Controllers
{
    [Area(nameof(Media))]
    public class MasterProgramCategoryTypeController : BaseController
    {
        string url = ApiUrl.ProgramCategoryTypeUrl;
        private readonly ApiClientFactory client;

        public MasterProgramCategoryTypeController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new ProgramCategoryTypeDetail();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<ProgramCategoryTypeDetail>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<ProgramCategoryTypeList>>(url) ?? new List<ProgramCategoryTypeList>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Save(ProgramCategoryTypeDetail model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<ProgramCategoryTypeDetail>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<ProgramCategoryTypeDetail>(url, model);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ProgramCategoryTypeDetail model) => Json(await client.DeleteApiResponse<ProgramCategoryTypeDetail>($"{url}/{model.Id}"));
    }
}