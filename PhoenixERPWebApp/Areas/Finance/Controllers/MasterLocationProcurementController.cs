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
    public class MasterLocationProcurementController : BaseController
    {
        string url = ApiUrl.MasterLocationProcurementUrl;
        private readonly ApiClientFactory client;

        public MasterLocationProcurementController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id)
        {
            var model = new MasterLocationProcurement();
            ViewBag.Header = "Master Location Procurement";
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<MasterLocationProcurement>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await client.Get<List<MasterLocationProcurement>>($"{url}") ?? new List<MasterLocationProcurement>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        [HttpPost]
        public async Task<IActionResult> Save(MasterLocationProcurement model)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                var response = new ApiResponse();

                if (!string.IsNullOrEmpty(model.Id))
                {
                    response = await client.PutApiResponse<MasterLocationProcurement>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<MasterLocationProcurement>(url, model);
                }

                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(MasterLocationProcurement model) => Json(await client.DeleteApiResponse<MasterLocationProcurement>($"{url}/{model.Id}"));
    }
}