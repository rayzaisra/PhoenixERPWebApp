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
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using PhoenixERPWebApp.Models.Finance.Transaction.ServiceReceipt;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class ServiceReceiptController : BaseController
    {
        string url = ApiUrl.ServiceReceipt;
        private readonly ApiClientFactory client;

        public ServiceReceiptController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }
                
        public async Task<IActionResult> Detail([FromQuery(Name = "id")] string Id)
        {
            var model = new ServiceReceipt();
            ViewBag.Header = "Service Receipt";
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.RecordID = Id;
                model = await client.Get<ServiceReceipt>($"{url}/{Id}");
            }           

            return View(model);
        }

        public async Task<IActionResult> ReadDetail([DataSourceRequest] DataSourceRequest request, string Id)
        {
            try
            {
                var model = await client.Get<List<ServiceReceiptDetail>>($"{url}/getDetail/{Id}") ?? new List<ServiceReceiptDetail>();

                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await client.Get<List<ServiceReceipt>>($"{url}") ?? new List<ServiceReceipt>();
              
                var data = model.ToDataSourceResult(request);
                return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public async Task<IActionResult> GetDataPO(string poId)
        {
            var model = await client.Get<ServiceReceipt>($"{url}/getPO/{poId}");
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> GetDataPODetail([DataSourceRequest] DataSourceRequest request, string poId)
        {
            var model = await client.Get<List<ServiceReceiptDetail>>($"{url}/getPODetail/{poId}") ?? new List<ServiceReceiptDetail>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(ServiceReceipt Header, List<ServiceReceiptDetail> Details)
        {
            try
            {
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                Header.ReceivedBy = requestor.EmployeeId;
                Header.ReceivedByName = requestor.EmployeeName;
                var response = new ApiResponse();
                var model = new ServiceReceiptDTO();
                
                model.Header = Header;
                model.Details = Details;
                if (!string.IsNullOrEmpty(Header.Id))
                {
                    response = await client.PutApiResponse<ServiceReceipt>(url, model);
                }
                else
                {
                    response = await client.PostApiResponse<ServiceReceipt>(url, model);
                }

                    
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message.ToString() });
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ServiceReceipt model) => Json(await client.DeleteApiResponse<ServiceReceipt>($"{url}/{model.Id}"));

    }
}