using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Attributes;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models.Finance.Transaction.F19DiageoPCE;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    //[MenuArea("590541e3-a932-4d29-b972-ddccb8efc1d7")]
    public class F19DiageoPCEController : BaseController
    {
        string url = ApiUrl.F19DiageoPCE;
        private readonly ApiClientFactory client;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = new List<F19DiageoPCEList>();
            model.Add(new F19DiageoPCEList()
            {
                Id = "001",
                Month = DateTime.Today,
                Nature = "Activation",
                PENumber = "DGO.001",
                Status = "Accrual",
                Brand = "JW Gold",
                PIC = "Petriana",
                BudgetOwner = "Dicky",
                CoreActivityDescription = "POSM_PRODUCTION",
                Description = "Johnnie Walker",
                Total = 5700000,
                ASF = 900000,
                VAT = 300000,
                GrandTotalIDR = 6900000,
                GrandTotalUSD = 650
            });
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new F19DiageoPCEDetail();
            ViewBag.Header = "F19 Diageo PCE Summary";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "F19 Diageo PCE Summary";
                model = await client.Get<F19DiageoPCEDetail>($"{url}/{Id}");
            }

            return View(model);
        }
    }
}