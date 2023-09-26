using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Attributes;
using Phoenix.WebExtension.RestApi;
using System.Xml;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.Finance;
using System.Data;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class RptBalanceSheetAffiliationController : Controller
    {
        string url = ApiUrl.RptBalanceSheet;
        private readonly ApiClientFactory client;
        public RptBalanceSheetAffiliationController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RptBalanceSheetAffiliationFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            model.CoaLevel = "5";
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        //public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, RptBalanceSheetAffiliationFilter Filter)
        //{
        //    if (Filter.Type == "Month")
        //    {
        //        Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
        //    }
        //    else if (Filter.Type == "Year")
        //    {
        //        Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
        //    }

        //    var model = new List<RptBalanceSheet>();
        //    var response = new ApiResponse();
        //    response = await client.PostApiResponse<List<RptBalanceSheet>>($"{url}/GetBalanceSheetAffiliation/", Filter);
        //    if (response.Data != null)
        //    {
        //        model = response.Data as List<RptBalanceSheet>;
        //    }

        //    var data = model.ToDataSourceResult(request);
        //    return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        public async Task<IActionResult> GetGenerateList(RptBalanceSheetAffiliationFilter Filter)
        {
            if (Filter.Type == "Month")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
            }
            else if (Filter.Type == "Year")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
            }

            var result = await client.PostApiResponse<DataTable>($"{url}/GetBalanceSheetAffiliation/", Filter);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}