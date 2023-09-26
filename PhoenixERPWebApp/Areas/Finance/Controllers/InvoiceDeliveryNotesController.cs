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
using PhoenixERPWebApp.Models.Finance.Transaction;
using PhoenixERPWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class InvoiceDeliveryNotesController : Controller
    {
        string url = ApiUrl.InvoiceDeliveryNotes;
        private readonly ApiClientFactory client;

        public InvoiceDeliveryNotesController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadIndex([DataSourceRequest] DataSourceRequest request, string status)
        {
            var data = await client.Get<List<InvoiceDeliveryNotes>>($"{url}/{status}") ?? new List<InvoiceDeliveryNotes>();
            var dataList = data.ToDataSourceResult(request);
            return Json(dataList, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string Id)
        {
            InvoiceDeliveryNotes model = new InvoiceDeliveryNotes();
            if (!string.IsNullOrEmpty(Id))
            {
                string _url = $"{url}/GetById/{Id}";
                model = await client.Get<InvoiceDeliveryNotes>(_url);
            }
            else
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                if (requestor != null)
                {
                    model.CreatedOn = DateTime.Now;
                    model.CreatedByName = requestor.EmployeeName;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            var model = await client.Get<List<AccountReceivable>>($"{url}/ReadDetail/{Id }") ?? new List<AccountReceivable>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetInvoiceChoiceList([DataSourceRequest] DataSourceRequest request, string invoiceid, string clientid, string brandid, string divisionid, string legalid)
        {
            var data = await client.Get<List<AccountReceivable>>($"{url}/GetInvoiceChoiceList/{invoiceid ?? "0"}/{clientid ?? "0"}/{brandid ?? "0"}/{divisionid ?? "0"}/{legalid ?? "0"}") ?? new List<AccountReceivable>();
            var model = data.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(InvoiceDeliveryNotes model, IFormFile fileDoc)
        {
            var response = new ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<InvoiceDeliveryNotes>(url, model, fileDoc);
            }
            else
            {
                response = await client.PostApiResponse<InvoiceDeliveryNotes>(url, model, fileDoc);
            }

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}