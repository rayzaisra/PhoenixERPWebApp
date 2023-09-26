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

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class RptBudgetOpexController : Controller
    {
        string url = ApiUrl.RptBudget;
        private readonly ApiClientFactory client;
        public RptBudgetOpexController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            var model = new RptBudgetFilter();
            model.CoaLevel = "5";
            model.Year = DateTime.Now.Year;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, RptBudgetFilter Filter)
        {
            var model = new List<RptBudget>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<RptBudget>>($"{url}/GetBudgetOpex/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<RptBudget>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}