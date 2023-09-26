using CodeMarvel.Infrastructure.WebNetCore;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.Transaction.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class InventoryGoodsReceiptController : BaseController
    {
        string url = ApiUrl.InventoryGoodsReceipt;
        string urlInvt = ApiUrl.Inventory;
        string urlFA = ApiUrl.FixedAssetUrl;
        private readonly ApiClientFactory client;

        public InventoryGoodsReceiptController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Id)
        {
            ViewBag.Header = "Goods Receipt";
            var model = new InventoryGoodsReceipt() { Status = StatusTransactionName.Draft, IsGenerated = false, IsNonInventory = false };
            if (!string.IsNullOrEmpty(Id)) model = await client.Get<InventoryGoodsReceipt>($"{url}/{Id}");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<InventoryGoodsReceipt>>(url) ?? new List<InventoryGoodsReceipt>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string poId, string grId)
        {
            var model = await client.Get<List<InventoryGoodsReceiptDetail>>($"{url}/getDetail/{poId}/{grId}") ?? new List<InventoryGoodsReceiptDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadHistory([DataSourceRequest] DataSourceRequest request, string poId)
        {
            var model = await client.Get<List<InventoryGoodsReceiptDetail>>($"{url}/getDetailHistory/{poId}") ?? new List<InventoryGoodsReceiptDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadInventoryDetail([DataSourceRequest] DataSourceRequest request, string grId)
        {
            var model = await client.Get<List<InventoryDetail>>($"{url}/getInventoryDetail/{grId}") ?? new List<InventoryDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadFixedAsset([DataSourceRequest] DataSourceRequest request, string grId)
        {
            var model = await client.Get<List<FixedAsset>>($"{url}/getFixedAsset/{grId}") ?? new List<FixedAsset>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetDataPO(string poId)
        {
            var model = await client.Get<InventoryGoodsReceipt>($"{url}/getPO/{poId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(InventoryGoodsReceipt model)
        {
            model.Status = StatusTransactionName.Draft;
            model.IsGenerated = false;
            
            return await Process(model, true); 
        }

        [HttpPost]
        public async Task<IActionResult> Submit(InventoryGoodsReceipt model)
        {
            model.Status = StatusTransactionName.Close;
            //if (!model.IsNonInventory)
            //{
            //    if (model.IsGenerated == false)
            //    {
            //        return Json(new
            //        {
            //            success = false,
            //            message = "Please generate first."
            //        });
            //    }
               
            //}
            return await Process(model, false); 
        }

        [HttpPost]
        public async Task<IActionResult> Generate(InventoryGoodsReceipt model)
        {
            model.Status = StatusTransactionName.Draft;
            model.IsGenerated = true;
            //return await Process(model, true); 

            foreach (var item in model.InventoryGoodsReceiptDetails)
            {
                if (item.ItemTypeName.ToUpper() == "INVENTORY")
                {
                    if (string.IsNullOrEmpty(item.InventoryId))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Please complete your Inventory Detail"
                        });
                    }
                }
                
            }

            var response = await client.PostApiResponse<InventoryGoodsReceipt>($"{url}/GeneratedInvoiceGoodReceipt", model);
            var data = response.Data as InventoryGoodsReceipt;
            var refreshGrid = new[] { "gridstatuslogHistoryList", "gridhistory", "gridinventory", "gridfixedasset" };
            var hdValue = new[] {
                    new { Id = "GoodsReceiptNumber", Value = data.GoodsReceiptNumber },
                    new { Id = "IsGenerated", Value = data.IsGenerated.ToString() }
                };

            return Json(new
            {
                success = response.Success,
                message = response.Message,
                refreshGrid = string.Join(";", refreshGrid),
                setHiddenValue = hdValue,
                data = data
            });
        }

        private async Task<IActionResult> Process(InventoryGoodsReceipt model, bool isDraft)
        {
            var response = new ApiResponse();
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            if (requestor.EmployeeId != model.PicId)
            {
                throw new Exception("That allow save is " + model.PicName + " Thanks !");
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<InventoryGoodsReceipt>(url, model);
                //return Json(new { success = response.Success, message = response.Message, url = Url.Action("Detail", "InventoryGoodsReceipt", new { Id = model.Id }) });
               
            }
            else
            {
                response = await client.PostApiResponse<InventoryGoodsReceipt>(url, model);
                //return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Detail)) });
            }

            //if (isDraft)
            //{
            //    var data = response.Data as InventoryGoodsReceipt;
            //    var refreshGrid = new[] { "gridstatuslogHistoryList", "gridhistory", "gridinventory", "gridfixedasset" };
            //    var hdValue = new[] {
            //        new { Id = "GoodsReceiptNumber", Value = data.GoodsReceiptNumber },
            //        new { Id = "IsGenerated", Value = data.IsGenerated.ToString() }
            //    };
            //    return Json(new
            //    {
            //        success = response.Success,
            //        message = response.Message,
            //        refreshGrid = string.Join(";", refreshGrid),
            //        setHiddenValue = hdValue,
            //        data = data
            //    });
            //}
            //else
            //{
            //    return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            //}
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [AcceptVerbs("Post")]
        public ActionResult EditingPOD_Update([DataSourceRequest] DataSourceRequest request, InventoryGoodsReceiptDetail igrd)
        {
            if (igrd != null && ModelState.IsValid) { }
            return Json(new[] { igrd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //[AcceptVerbs("Post")]
        //public ActionResult EditingInv_Update([DataSourceRequest] DataSourceRequest request, InventoryDetail invd)
        //{
        //    if (invd != null && ModelState.IsValid) { }
        //    return Json(new[] { invd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}

        public async Task<IActionResult> EditingInv_Update([DataSourceRequest] DataSourceRequest request, InventoryDetail invd)
        {
            if (invd != null && ModelState.IsValid) { await client.Post($"{url}/UpdateInventoryDetail", invd); }
            
            return Json(new[] { invd }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        public ActionResult EditingFA_Update([DataSourceRequest] DataSourceRequest request, FixedAsset fa)
        {
            if (fa != null && ModelState.IsValid) { }
            return Json(new[] { fa }.ToDataSourceResult(request, ModelState), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        } 
    }
}