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
using Phoenix.WebExtension.Attributes;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PhoenixERPWebApp.Models.Finance.Transaction.GeneralAdjustment;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class GeneralAdjustmentController : BaseController
    {
        string url = ApiUrl.GeneralAdjustment;
        private readonly ApiClientFactory client;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new GeneralAdjustmentDetail();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<GeneralAdjustmentDetail>($"{url}/{Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            //var model = await client.Get<List<AccountPayable>>(url) ?? new List<AccountPayable>();
            var model = new List<GeneralAdjustmentList>();
            model.Add(new GeneralAdjustmentList()
            {
                Id = "001",
                EVDNo = "JV.17010001",
                Date = DateTime.Now,
                Client = "PT. Nestle",
                Vendor = "PT. Bintang",
                Type = "Vendor",
                Amount = 345234234,
                ReceiptFrom = "Daya Maha Berkarya, PT.",
                Remarks = "Lorem ipsum",
                Status = "Posted"
            });
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request)
        {
            //var model = await client.Get<List<AccountPayable>>(url) ?? new List<AccountPayable>();
            var model = new List<GeneralAdjustmentDetailList>();
            model.Add(new GeneralAdjustmentDetailList()
            {
                Id = "001",
                ACCNo = "11303",
                Name = "Accrued Fee",
                Desc = "Lorem ipsum",
                Debit = 123123123,
                Credit = 5000000,
                Division = "SKOR",
                JobId = "Job.123"
            });
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}