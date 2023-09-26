using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.UI;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Finance.Transaction;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoenixERPWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class AccountReceivableController : BaseController
    {
        private readonly ApiClientFactory client;
        string ar_url = ApiUrl.ARClientUrl;

        public AccountReceivableController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadIndex([DataSourceRequest] DataSourceRequest request, FilterInvoiceClient Filter)
        {
            var model = new List<InvoiceClient>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<InvoiceClient>>($"{ar_url}/GetARList/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<InvoiceClient>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string id)
        {
            var model = await client.Get<InvoiceClient>($"{ar_url}/{id}") ?? new InvoiceClient();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(InvoiceClient model, IFormFile fileVatDoc)
        {
            if (model.InvoiceDeliveryDate != null)
            {
                if (fileVatDoc != null) { model.VatDocFileName = fileVatDoc.FileName; }
                var response = await client.PutApiResponse<InvoiceClient>(ar_url, model, fileVatDoc);
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            else
            {
                return Json(new { success = false, message = "Please fill in the delivery notes first before proceeding to the account receivable process" });
            }
        }

        public async Task<IActionResult> ReadJournal([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<Journal>>($"{ar_url}/GetDataJournal/{Id}") ?? new List<Journal>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}