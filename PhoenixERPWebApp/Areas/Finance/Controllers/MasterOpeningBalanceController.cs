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
    [Area(nameof(Finance))]
    public class MasterOpeningBalanceController : Controller
    {
        string url = ApiUrl.MasterOpeningBalanceUrl;
        private readonly ApiClientFactory client;

        public MasterOpeningBalanceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new MasterOpeningBalanceFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.FilterCurrencyId = currencyDefault.Id;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, MasterOpeningBalanceFilter Filter)
        {
            var model = new List<MasterOpeningBalance>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<MasterOpeningBalance>>($"{url}/GetData", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<MasterOpeningBalance>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> Update([DataSourceRequest] DataSourceRequest request, MasterOpeningBalance model)
        {
            if (model != null && ModelState.IsValid)
            {
                var result = await client.PostApiResponse<MasterOpeningBalance>(url, model);
                model = result.Data as MasterOpeningBalance;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}