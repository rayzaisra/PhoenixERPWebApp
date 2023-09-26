using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Enum;
using PhoenixERPWebApp.Models.Finance.Transaction.GoodReturn;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class GoodReturnController : Controller
    {
        string url = ApiUrl.GoodReturn;
        //string urlInvt = ApiUrl.Inventory;
        //string urlFA = ApiUrl.FixedAssetUrl;
        private readonly ApiClientFactory client;

        public GoodReturnController(ApiClientFactory client)
        {
            this.client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                //List<InventoryGoodsReceipt> model = new List<InventoryGoodsReceipt>();

                var model = await client.Get<List<InventoryGoodsReturn>>($"{url}") ?? new List<InventoryGoodsReturn>();
                //if (response != null)
                //{
                //    model = CastResponseData<List<InventoryGoodsReceipt>>(response.Data);
                //}

                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> Detail(string Id)
        {
            var model = new InventoryGoodsReturn();
            ViewBag.Header = "Detail";
            var isCurrentApproval = false;
            ViewBag.processApprove = false;
            ViewBag.IsSubmitter = false;
            var IsAdministrator = false;
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<InventoryGoodsReturn>($"{url}/{Id}");
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                isCurrentApproval = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
                IsAdministrator = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAdministrator");
                ViewBag.processApprove = isCurrentApproval && (model.Status == GoodRequestStatus.StatusName(2));
                ViewBag.IsSubmitter = IsAdministrator || (requestor.EmployeeName == model.RequestedBy);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(InventoryGoodsReturn model)
        {
            if (model.StatusDesc == "Submit")
            {
                model.Status = "2";
                return await Process(model, false);
            }
            else
            {
                return await Approve(model);
            }
            
        }

        private async Task<IActionResult> Process(InventoryGoodsReturn model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<InventoryGoodsReturn>(url, model);

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(InventoryGoodsReturn model)
        {
            if (model.StatusDesc == "Rejected")
            {
                model.Status = "4";
            }
            else
            {
                model.Status = "3";
            }
            var response = await client.PutApiResponse<InventoryGoodsReturn>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }
    }
}