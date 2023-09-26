using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Kendo.Mvc.Extensions;
using System;
using Microsoft.AspNetCore.Http;
using PhoenixERPWebApp.Models.Pm.MyDam;
using Phoenix.WebExtension;
using System.IO;

namespace PhoenixERPWebApp.Areas.Pm.Controllers
{

    [Area(nameof(Pm))]
    public class MyDamController : Controller
    {
        string url = ApiUrl.MyDam;
        private readonly ApiClientFactory client;

        public MyDamController(ApiClientFactory client)
        {
            this.client = client;
        }


        public async Task<IActionResult> Index()
        {
            var UserLogin = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            ViewBag.UserLogin = UserLogin.Id;
            ViewBag.IsAdministrator = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAdministrator");
            return View();
        }

        public IActionResult IndexApproval()
        {
            return View();
        }
        public async Task<IActionResult> Detail(string Id)
        {
            var UserLogin = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            ViewBag.UserLogin = UserLogin.Id;

            var model = new MyDam();
            ViewBag.Header = "Detail";
            ViewBag.processApprove = await client.Get<bool>($"{ApiUrl.ManageTransApproval}/IsCurrentApproval/{Id}");
            if (!string.IsNullOrEmpty(Id))
            {
                model = await client.Get<MyDam>($"{url}/{Id}");
            }
            return View(model);
        }

        public async Task<IActionResult> GetMyDAM([DataSourceRequest] DataSourceRequest request)
        {
            string baseUrl = "";
            string urlWeb = DataConfiguration.Configuration.AppConfiguration.ApiServerUrl;
            if (urlWeb.EndsWith("/"))
                baseUrl = urlWeb.Replace("api/", "");

            // string baseUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

            var model = await client.Get<List<MyDam>>(url) ?? new List<MyDam>();
            foreach (var item in model)
            {
                item.FilePath = baseUrl + "/" + item.FilePath;
            }

            var data = model.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        public async Task<IActionResult> GetMyDAMByFilename([DataSourceRequest] DataSourceRequest request, string filename)
        {
            string baseUrl = "";
            string urlWeb = DataConfiguration.Configuration.AppConfiguration.ApiServerUrl;
            if (urlWeb.EndsWith("/"))
                baseUrl = urlWeb.Replace("api/", "");

            // string baseUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

            var model = await client.Get<List<MyDam>>($"{url}/GetByFilename/{ filename}") ?? new List<MyDam>();
            foreach (var item in model)
            {
                item.FilePath = baseUrl + "/" + item.FilePath;
            }

            var data = model.ToDataSourceResult(request);
            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string SelectedId)
        {
            RequestMyDam model = new RequestMyDam();
            model.SelectedId = SelectedId;
            model.Status = StatusTransaction.WaitingApproval;
            return await Process(model, false);
        }

        [HttpPost]
        public async Task<IActionResult> Move(string SelectedId, string FolderId)
        {
            RequestMyDam model = new RequestMyDam();
            model.SelectedId = SelectedId;
            model.FolderId = FolderId;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<MyDam>($"{url}/Move", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }



        private async Task<IActionResult> Process(RequestMyDam model, bool isDraft)
        {
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            model.RequestBy = requestor.Id;
            model.RequestDate = DateTime.Now;
            var response = new Phoenix.WebExtension.RestApi.ApiResponse();
            response = await client.PostApiResponse<MyDam>(url, model);

            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(MyDam model)
        {
            if (model.StatusDesc == "Rejected")
            {
                model.Status = 4;
            }
            else
            {
                model.Status = 3;
            }
            var response = await client.PutApiResponse<MyDam>($"{url}/Approve", model);
            return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(IndexApproval)) });
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<MyDam>>($"{url}/GetMyDamWaitingApproval") ?? new List<MyDam>();
            foreach (var item in model)
            {
                item.StatusDesc = StatusTransaction.StatusName(item.Status);
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string SelectedId) => Json(await client.DeleteApiResponse<MyDam>($"{url}/{SelectedId}"));

        

    }
}

