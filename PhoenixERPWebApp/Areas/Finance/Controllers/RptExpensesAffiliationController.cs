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
    public class RptExpensesAffiliationController : Controller
    {
        string url = ApiUrl.RptExpensesAffiliation;
        private readonly ApiClientFactory client;
        public RptExpensesAffiliationController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RptExpensesAffiliationFilter();
            var currencyDefault = await client.Get<Currency>($"{ApiUrl.CurrencyUrl}/GetCurrencyDefault");
            model.CurrencyId = currencyDefault.Id;
            model.CoaLevel = "5";
            model.EndingDate = DateTime.Now;
            return View(model);
        }

        public async Task<IActionResult> GetGenerateList(RptExpensesAffiliationFilter Filter)
        {
            if (Filter.Type == "Month")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year}/{Filter.EndingDate.Value.Month + 1}/1").AddDays(-1);
            }
            else if (Filter.Type == "Year")
            {
                Filter.EndingDate = Convert.ToDateTime($"{Filter.EndingDate.Value.Year + 1}/1/1").AddDays(-1);
            }

            var result = await client.PostApiResponse<DataTable>($"{url}/GetExpensesAffiliation/", Filter);
            return Json(result, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}