using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using System.Xml;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class RptPreFinancingController : Controller
    {
        string url = ApiUrl.RptPreFinancing;
        private readonly ApiClientFactory client;
        public RptPreFinancingController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RptPreFinancingFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, RptPreFinancingFilter Filter)
        {
            var model = new List<RptPreFinancing>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<RptPreFinancing>>($"{url}/GetData/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<RptPreFinancing>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}