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
using Phoenix.WebExtension.RestApi;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class AccountPayableController : BaseController
    {
        string url = ApiUrl.InvoiceReceivable;
        private readonly ApiClientFactory client;

        public AccountPayableController(ApiClientFactory client)
        {
            this.client = client;
        }

        public ActionResult Index()
        {
            var model = new FilterInvoiceReceivable() { Status = InvoiceReceivedStatus.Posted, EndingDate = DateTime.Now };
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, FilterInvoiceReceivable Filter)
        {
            var model = new List<AccountPayable>();
            var response = new ApiResponse();
            response = await client.PostApiResponse<List<AccountPayable>>($"{url}/GetAPList/", Filter);
            if (response.Data != null)
            {
                model = response.Data as List<AccountPayable>;
            }

            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}