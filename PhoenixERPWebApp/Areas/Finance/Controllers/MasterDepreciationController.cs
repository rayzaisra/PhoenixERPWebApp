using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class MasterDepreciationController : BaseController
    {
        string url = ApiUrl.MasterDepreciationUrl;
        private readonly ApiClientFactory client;

        public MasterDepreciationController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id)
        {
            var model = new MasterDepreciation();
            ViewBag.Header = "Master Depreciation";
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<MasterDepreciation>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await client.Get<List<MasterDepreciation>>($"{url}") ?? new List<MasterDepreciation>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        [HttpPost]
        public async Task<IActionResult> Save(MasterDepreciation model)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();

                if (!string.IsNullOrEmpty(model.Id))
                {
                    response = await client.PutApiResponse<MasterDepreciation>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<MasterDepreciation>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MasterDepreciation model) => Json(await client.DeleteApiResponse<MasterDepreciation>($"{url}/{model.Id}"));
    }
}