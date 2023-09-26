using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class TemporaryAccountPayableController : BaseController
    {
        string url = ApiUrl.InvoiceReceivable;
        private readonly ApiClientFactory client;

        public TemporaryAccountPayableController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> Index()
        {
            var group = await client.Get<List<TmUserApp>>($"{ApiUrl.ManageUserUrl}/GetByUserLogin");
            var accountingSupervisor = group.Where(x => x.GroupId == GroupAcess.AccountingSupervisor).Count() > 0;
            var taxSupervisor = group.Where(x => x.GroupId == GroupAcess.TaxSupervisor).Count() > 0;
            var financeManager = group.Where(x => x.GroupId == GroupAcess.FinanceManager).Count() > 0;
            var model = new InvoiceReceivable() { InvoiceStatusId = accountingSupervisor ? InvoiceReceivedStatus.TAP : taxSupervisor ? InvoiceReceivedStatus.TAXTAP : financeManager ? InvoiceReceivedStatus.OpenAP : InvoiceReceivedStatus.TAP };
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, string status, string businessUnit, string legalEntity, string invoiceNumber, string invoiceVendor)
        {
            var model = await client.Get<List<InvoiceReceivable>>($"{url}/GetList/{(status ?? "0")}/{(businessUnit ?? "0")}/{(legalEntity ?? "0")}/{(invoiceNumber ?? "0")}/{(invoiceVendor ?? "0")}") ?? new List<InvoiceReceivable>();
            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}