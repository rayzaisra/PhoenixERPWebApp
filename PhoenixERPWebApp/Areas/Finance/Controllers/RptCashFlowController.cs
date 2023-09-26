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
using System.Data;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class RptCashFlowController : Controller
    {
        string url = ApiUrl.RptCashFlowUrl;
        private readonly ApiClientFactory client;

        public RptCashFlowController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RptCashFlowFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            model.StartDate = DateTime.Now;
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        public async Task<IActionResult> GetGenerateList(RptCashFlowFilter Filter)
        {
            var result = await client.PostApiResponse<DataTable>($"{url}/GetData/", Filter);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}