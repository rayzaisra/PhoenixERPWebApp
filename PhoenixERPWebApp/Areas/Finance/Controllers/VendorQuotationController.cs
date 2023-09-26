using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.RestApi;
using Kendo.Mvc.UI;
using PhoenixERPWebApp.Models.Finance.Transaction;
using Newtonsoft.Json.Serialization;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using PhoenixERPWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class VendorQuotationController : Controller
    {
        string url = ApiUrl.VendorQuotation;
        string urlRFQ = ApiUrl.RequestForQuotationUrl;
        private readonly ApiClientFactory client;
        public VendorQuotationController(ApiClientFactory _client)
        {
            this.client = _client;
        }
        public async Task<IActionResult> Index()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MainserviceCategory";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            ViewBag.Category = model;
            return View();
        }

        public IActionResult Detail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            ViewBag.EntityID = "430b0078-b659-bc95-30f3-b5d8b22cfe2c";
            return View();
        }
        public IActionResult DetailMultiple([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            ViewBag.EntityID = "430b0078-b659-bc95-30f3-b5d8b22cfe2c";
            return View();
        }
    }
}