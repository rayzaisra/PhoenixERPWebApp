using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Sessions;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.Extensions.Configuration;
using CodeMarvel.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Kendo.Mvc.UI;
using PhoenixERPWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using Kendo.Mvc.Extensions;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area(nameof(Finance))]
    public class GeneralLedgerDetailController : BaseController
    {
        string url = ApiUrl.GeneralLedgerDetailUrl;
        private readonly ApiClientFactory client;
        public GeneralLedgerDetailController(ApiClientFactory _client)
        {
            this.client = _client;
        }
        public IActionResult Index()
        {
            var model = new GeneralLedgerDetailSearch()
            {
                PeriodeFrom = new DateTime(DateTime.Now.Year, 1, 1),
                PeriodeTo = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                Level = 5
            };
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, GeneralLedgerDetailSearch search)
        {
            var model = await client.PostApiResponse<List<GeneralLedgerDetail>>($"{url}/GetList", search);
            var List = new List<GeneralLedgerDetail>();
            if (model.Data != null) List = model.Data as List<GeneralLedgerDetail>;
            var data = List.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public ActionResult Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }
    }
}