using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class LoanCategoryController : Controller
    {
        string url = ApiUrl.LoanCategoryUrl;
        private readonly ApiClientFactory client;

        public LoanCategoryController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string Id)
        {
            var model = new LoanCategory();
            ViewBag.Header = "Create";

            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.Header = "Update";
                model = await client.Get<LoanCategory>($"{url}/{Id}");
                if (model.is_Corporate_Loan == true)
                {
                    model.IsCorporateLoan = "1";
                }
                else
                {
                    model.IsCorporateLoan = "0";
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<LoanCategory>>(url) ?? new List<LoanCategory>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LoanCategory model)
        {
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            if (model.IsCorporateLoan == "1")
            {
                model.is_Corporate_Loan = true;
            }
            else
            {
                model.is_Corporate_Loan = false;
            }
            if (!string.IsNullOrEmpty(model.Id))
            {
                response = await client.PutApiResponse<LoanCategory>(url, model);
            }
            else
            {
                response = await client.PostApiResponse<LoanCategory>(url, model);
            }
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(LoanCategory model) => Json(await client.DeleteApiResponse<LoanCategory>($"{url}/{model.Id}"));
    }
}