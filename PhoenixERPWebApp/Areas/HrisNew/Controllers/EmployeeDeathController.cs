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
    public class EmployeeDeathController : Controller
    {
        string url = ApiUrl.DeathUrl;
        private readonly ApiClientFactory client;

        public EmployeeDeathController(ApiClientFactory client)
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
                model.TerminationType = TerminationType.Death;
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
        public async Task<IActionResult> SubmitToHC(Termination model, IFormFile file, IFormFile fileInterview, IFormFile filePolice)
        {
            model.Status = StatusTransaction.Draft; //tidak di pakai sehingga harus di set draft untuk tidak terkena validasi approval
            model.IsHcProcess = true;
            return await Process(model, file, fileInterview, filePolice, false);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Termination model, IFormFile file, IFormFile fileInterview, IFormFile filePolice)
        {
            model.Status = StatusTransaction.Draft; //tidak di pakai sehingga harus di set draft untuk tidak terkena validasi approval
            return await Process(model, file, fileInterview, filePolice, true);
        }

        private async Task<IActionResult> Process(Termination model, IFormFile file, IFormFile fileInterview, IFormFile filePolice, bool isDraft)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (!string.IsNullOrEmpty(model.Id))
            {
                if (model.TerminationType == TerminationType.Retired)
                {
                    response = await client.PutApiResponse<Termination>(ApiUrl.RetiredUrl, model);
                }
                else if (model.TerminationType == TerminationType.Sanction)
                {
                    response = await client.PutApiResponse<Termination>(ApiUrl.SanctionUrl, model, fileInterview, filePolice);
                }
                else
                {
                    response = await client.PutApiResponse<Termination>(url, model, file);
                }
            }
            else
            {
                if (model.TerminationType == TerminationType.Retired)
                {
                    response = await client.PostApiResponse<Termination>(ApiUrl.RetiredUrl, model);
                }
                else if (model.TerminationType == TerminationType.Sanction)
                {
                    response = await client.PostApiResponse<Termination>(ApiUrl.SanctionUrl, model, fileInterview, filePolice);
                }
                else
                {
                    response = await client.PostApiResponse<Termination>(url, model, file);
                }
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