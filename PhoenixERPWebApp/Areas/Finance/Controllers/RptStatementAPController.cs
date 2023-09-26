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
    public class RptStatementAPController : Controller
    {
        string url = ApiUrl.RptStatementAP;
        private readonly ApiClientFactory client;
        public RptStatementAPController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, VwStatementOfApFilter Filter)
        {
            var model = new List<VwStatementOfAp>();
            var response = new ApiResponse();

            if (!string.IsNullOrEmpty(Filter.LegalId)) //harus pilih legal entity terlebih dahulu.
            {
                response = await client.PostApiResponse<List<VwStatementOfAp>>($"{url}/GetData/", Filter);
                if (response.Data != null)
                {
                    model = response.Data as List<VwStatementOfAp>;
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
