using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class EmployeeContractController : Controller
    {
        string url = ApiUrl.ContractUrl;
        private readonly ApiClientFactory client;

        public EmployeeContractController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new Termination();
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<Termination>($"{url}/{Id}");
                ViewBag.isRequestor = !(model.IsHcProcess ?? false);//(model.Status == StatusTransaction.Draft || model.Status == StatusTransaction.Rejected);
            }
            else
            {
                ViewBag.Header = "Create";
                var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
                model.requestEmployee = requestor;
                model.Requestor = requestor.EmployeeId;
                model.RequestDate = DateTime.Now;
                ViewBag.isRequestor = true;
                model.infoEmployee = new InfoEmployee();
                model.TerminationType = TerminationType.EndOfContract;
            }

            model.Reason = HttpUtility.HtmlDecode(model.Reason);
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Termination>>(url) ?? new List<Termination>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> submitToHC(Termination model)
        {
            model.Status = StatusTransaction.Draft; //tidak di pakai sehingga harus di set draft untuk tidak terkena validasi approval
            model.IsHcProcess = true;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Termination model)
        {
            model.Status = StatusTransaction.Draft; //tidak di pakai sehingga harus di set draft untuk tidak terkena validasi approval
            return await Process(model, true);
        }

        private async Task<IActionResult> Process(Termination model, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<Termination>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<Termination>(url, model);
            }

            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Termination });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Termination model) => Json(await client.DeleteApiResponse<Termination>($"{url}/{model.Id}"));
    }
}