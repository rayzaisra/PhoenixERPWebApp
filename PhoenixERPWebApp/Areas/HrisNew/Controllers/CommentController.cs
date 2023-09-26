using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{

    [Area(nameof(HrisNew))]
    public class CommentController : Controller
    {
        string url = ApiUrl.CommentUrl;
        private readonly ApiClientFactory client;

        public CommentController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id, bool isServiceReq)
        {
            var model = new EmployeeServiceRequest();
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<EmployeeServiceRequest>($"{url}/{Id}");
            }
            else
            {
                ViewBag.Header = "Create";
                var request = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = request;
                model.CreatedBy = request.EmployeeId;
                model.RequestDate = DateTime.Now;
            }

            model.Remarks = HttpUtility.HtmlDecode(model.Remarks);
            model.IsServiceReq = isServiceReq;
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<EmployeeServiceRequest>>(url) ?? new List<EmployeeServiceRequest>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(EmployeeServiceRequest model)
        {
            model.Status = EmployeeServiceRequestStatus.Closed;
            var response = await client.PutApiResponse<EmployeeServiceRequest>(url, model);
            if (model.IsServiceReq)
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action("Index", new { controller = "ServiceRequest", area = "HrisNew" }) });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(EmployeeServiceRequest model) => Json(await client.DeleteApiResponse<EmployeeServiceRequest>($"{url}/{model.Id}"));
    }
}