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
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Hrisnew.Controllers
{
    [Area(nameof(Hrisnew))]
    public class GroupController : Controller
    {
        string url = ApiUrl.Groupurl;
        private readonly ApiClientFactory client;

        public GroupController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new BusinessUnit();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<BusinessUnit>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MappingBusinessUnit>>(url) ?? new List<MappingBusinessUnit>();
            foreach (MappingBusinessUnit tr in model) {
                tr.Id = Guid.NewGuid().ToString();
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadSp([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MappingBusinessUnit>>($"{ApiUrl.Groupurl}/Getsp") ?? new List<MappingBusinessUnit>();
            foreach (MappingBusinessUnit tr in model)
            {
                tr.Id = Guid.NewGuid().ToString();
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(BusinessUnit model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<BusinessUnit>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<BusinessUnit>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(BusinessUnit model) => Json(await client.DeleteApiResponse<BusinessUnit>($"{url}/{model.IdGroup}"));
    }
}