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
    public class RptBillingAndRevenueSummaryController : Controller
    {
        string url = ApiUrl.RptBillingAndRevenue;
        private readonly ApiClientFactory client;

        public RptBillingAndRevenueSummaryController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RptBillingAndRevenueFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            //model.MainServiceCategoryId = MainServiceCategoryId.Production;
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, RptBillingAndRevenueFilter Filter)
        {
            if (Filter.DateType == "Month")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
            }
            else if (Filter.DateType == "Year")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
            }

            var model = new List<RptBillingAndRevenueSummary>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<RptBillingAndRevenueSummary>>($"{url}/GetSummary/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<RptBillingAndRevenueSummary>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}