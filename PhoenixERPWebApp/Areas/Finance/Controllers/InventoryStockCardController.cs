using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.Transaction.Inventory;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class InventoryStockCardController : BaseController
    {
        string uniqueDetailInv = "DETAILINVSTOCK";
        string url = ApiUrl.Inventory;
        private readonly ApiClientFactory client;
        public InventoryStockCardController(ApiClientFactory client)
        {
            this.client = client;
        }
        public async Task<IActionResult> Index()
        {
            var model = new Inventory();
            return View(model);
        }

        public async Task<IActionResult> GetDataInventoryStockCard(string id)
        {
            var model = await client.Get<Inventory>($"{url}/GetDataInventoryStockCard/{id}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadInventoryStockCardDetail([DataSourceRequest] DataSourceRequest request, string invId)
        {
            try
            {
                var model = await client.Get<List<InventoryDetail>>($"{url}/GetDetailForInventoryStockCard/{invId}") ?? new List<InventoryDetail>();
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public IActionResult Detail([FromQuery(Name = "id")] string Id)
        {
            ViewBag.RecordID = Id;
            return View();
        }
    }
}