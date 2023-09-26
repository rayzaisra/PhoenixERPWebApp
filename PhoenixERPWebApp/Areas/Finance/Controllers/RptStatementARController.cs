using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class RptStatementARController : Controller
    {
        string url = ApiUrl.RptStatementAR;
        private readonly ApiClientFactory client;
        public RptStatementARController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, VwStatementOfArFilter Filter)
        {
            var model = new List<VwStatementOfAr>();
            var response = new ApiResponse();

            if (!string.IsNullOrEmpty(Filter.LegalId)) //harus pilih legal entity terlebih dahulu.
            {
                response = await client.PostApiResponse<List<VwStatementOfAr>>($"{url}/GetData/", Filter);
                if (response.Data != null)
                {
                    model = response.Data as List<VwStatementOfAr>;
                }
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public ActionResult Download(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }
    }
}