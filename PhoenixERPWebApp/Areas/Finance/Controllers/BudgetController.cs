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
using Kendo.Mvc.UI;
using PhoenixERPWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using Kendo.Mvc.Extensions;
using PhoenixERPWebApp.Models.Finance.Transaction.Budget;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Phoenix.WebExtension.Extentions;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class BudgetController : BaseController
    {
        string url = ApiUrl.BugetUrl;
        private readonly ApiClientFactory client;
       

        public BudgetController(ApiClientFactory client)
        {
            this.client = client;
        }


        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Detail(string id)
        {
            var model = new TypeBudgetHdr();
            if (!string.IsNullOrEmpty(id))
            {
                model = await client.Get<TypeBudgetHdr>($"{url}/GetIdWithName/{id}");
            }
            ViewBag.employee_basic_info_id = HttpContext.Session.GetString(AppConstants.EmployeeBasicInfoId);
            //return View();
            return View(model);
        }

        public async Task<ActionResult> GetMasterFinancialYear()
        {
            var getUrl = $"{ApiUrl.dropdownUrl}/MediaType";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public async Task<ActionResult> GetFiscalyearList()
        {
            var getUrl = $"{ApiUrl.MasterFinancialYearURL}";
            var model = await client.Get<List<SelectListItem>>(getUrl) ?? new List<SelectListItem>();
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Save(TypeBudgetHdr model, IFormFile file)
        {
            var response = new ApiResponse();
            response = await client.PostApiResponse<TypeBudgetHdr>($"{url}/uploaddata", model, file);
            return Json(new { success = response.Success, message = response.Message, url = @Url.Action(nameof(Index)) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(TypeBudgetHdr model) => Json(await client.DeleteApiResponse<TypeBudgetHdr>($"{url}/{model.Id}"));

        public async Task<IActionResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<TypeBudgetHdr>>($"{url}/GetWithName") ?? new List<TypeBudgetHdr>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadDataDetail([DataSourceRequest] DataSourceRequest request,string headerId)
        {
            var model = await client.Get<List<TypeBudget>>($"{url}/GetDetailTypeBudget/{headerId}") ?? new List<TypeBudget>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

    }
}