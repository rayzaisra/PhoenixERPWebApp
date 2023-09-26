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
    public class RptTrialBalanceController : Controller
    {
        string url = ApiUrl.RptTrialBalanceUrl;
        private readonly ApiClientFactory client;
        public RptTrialBalanceController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RptTrialBalanceFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            model.CoaLevel = "5";
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, RptTrialBalanceFilter Filter)
        {
            if (Filter.Type == "Month")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
            }
            else if (Filter.Type == "Year")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
            }

            var model = new List<RptTrialBalance>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<RptTrialBalance>>($"{url}/GetData/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<RptTrialBalance>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        } 

        //public async Task<ActionResult> GetTotal(RptTrialBalanceFilter Filter)
        //{
        //    var model = new RptTrialBalanceValue();
        //    var response = new ApiResponse();
        //    response = await client.PostApiResponse<RptTrialBalanceValue>($"{url}/GetTotal/", Filter);
        //    if (response.Data != null)
        //    {
        //        model = response.Data as RptTrialBalanceValue;
        //    }
        //    return Json(model);
        //}
    }
}