using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class MasterVendorController : Controller
    {
        string url = ApiUrl.MasterVendorUrl;
        string urlCurrency = ApiUrl.CurrencyUrl;
        private readonly ApiClientFactory client;
        static string paramValueTemp = string.Empty;
        static string paramRequestTemp = string.Empty;
        static DataSourceRequest requestTemp;
        public MasterVendorController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            if (paramRequestTemp == "edit")
            {
                paramValueTemp = "1";
            }
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new Models.Company();
            model.IsClient = false;
            ViewBag.Header = "Create";


            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<Models.Company>($"{url}/{Id}");
            }

            var Currency = await client.Get<Currency>($"{urlCurrency}/GetCurrencyDefault");
            if (Currency != null)
            {
                model.CurrencyId = Currency.Id;
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Models.Company>>(url) ?? new List<Models.Company>();
            var data = model.ToDataSourceResult(request);
            if (paramValueTemp != "")
            {
                data = model.ToDataSourceResult(requestTemp);
                paramRequestTemp = "";
                paramValueTemp = "";
            }
            else
            {
                requestTemp = request;
            }

            //var result = new { data = data, ParamVal = paramValueTemp, ParamColumn = paramColumnTemp };
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Models.Company model, IFormFile fileImageTaxScan, IFormFile fileImageLegalScan)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Models.Company>(url, model, fileImageTaxScan, fileImageLegalScan);
                if (requestTemp != null)
                {
                    paramRequestTemp = "edit";
                }
            }
            else
            {
                response = await client.PostApiResponse<Models.Company>(url, model, fileImageTaxScan, fileImageLegalScan);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Models.Company model) => Json(await client.DeleteApiResponse<Models.Company>($"{url}/{model.Id}"));


        public async Task<IActionResult> GetById(string id)
        {
            var model = await client.Get<Models.Company>($"{url}/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> CheckValidationAliasName(string name)
        {
            var model = await client.Get<Models.Company>($"{url}/CheckValidationAliasName/{name}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}