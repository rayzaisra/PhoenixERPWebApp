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
using PhoenixERPWebApp.Models.Finance.Transaction.PaymentRequest;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class BankPaymentController : Controller
    {
        string url = ApiUrl.BankPaymentUrl;
        private readonly ApiClientFactory client;

        public BankPaymentController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, FilterPaymentRequest Filter)
        {
            var model = new List<BankPaymentList>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<BankPaymentList>>($"{url}/GetBankPaymentList/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<BankPaymentList>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}