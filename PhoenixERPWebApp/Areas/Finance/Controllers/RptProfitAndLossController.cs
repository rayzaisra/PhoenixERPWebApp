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
    public class RptProfitAndLossController : Controller
    {
        string url = ApiUrl.RptProfitAndLossUrl;
        private readonly ApiClientFactory client;

        public RptProfitAndLossController(ApiClientFactory client)
        {
            this.client = client;
        }
        
        public async Task<IActionResult> Index()
        {
            var model = new RptProfitAndLossFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            model.CoaLevel = "5";
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        //public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, RptProfitAndLossFilter Filter)
        //{
        //    if (Filter.Type == "Month")
        //    {
        //        Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
        //    }
        //    else if (Filter.Type == "Year")
        //    {
        //        Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
        //    }

        //    var model = new List<RptProfitAndLoss>();
        //    var response = new ApiResponse();
        //    response = await client.PostApiResponse<List<RptProfitAndLoss>>($"{url}/GetData/", Filter);
        //    if (response.Data != null)
        //    {
        //        model = response.Data as List<RptProfitAndLoss>;
        //    }

        //    var data = model.ToDataSourceResult(request);
        //    return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        public async Task<IActionResult> GetGenerateList(RptProfitAndLossFilter Filter)
        {
            if (Filter.Type == "Month")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
            }
            else if (Filter.Type == "Year")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
            }

            var result = await client.PostApiResponse<DataTable>($"{url}/GetData/", Filter);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}