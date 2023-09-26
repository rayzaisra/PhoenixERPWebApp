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
using Microsoft.AspNetCore.Http;
using Phoenix.WebExtension.RestApi;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class BudgetManagementController : BaseController
    {
        string url = ApiUrl.MediaPlanTV;
        private readonly ApiClientFactory client;

        public BudgetManagementController(ApiClientFactory client) {
            this.client = client;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Process(IFormFile file)
        {
            var response = new ApiResponse();
            var url = "http://localhost:5050/api/finance/BudgetManagement/uploaddata";
            response = await client.PostApiResponse<string>(url, null, file);
            //var media = response.Data as MediaPlanTV;
            return Json(new { success = response.Success, message = response.Message, url = @Url.Action("addedit", "MediaPlanTV") + "?Id=1" });
        }


    }
}