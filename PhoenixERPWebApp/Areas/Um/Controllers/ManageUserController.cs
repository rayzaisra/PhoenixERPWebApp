using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phoenix.WebExtension.Attributes;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageUserController : Controller
    {
        private readonly ApiClientFactory client;
        public ManageUserController(ApiClientFactory _client)
        {
            this.client = _client;
        }

        private string _uniqueName = "SETTINGUSER";
        public IActionResult Index()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult ChangePassword()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }

        public IActionResult Profile()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }
        public IActionResult UpdateProfile()
        {
            ViewBag.PageUniqueName = _uniqueName;
            return View();
        }

        public async Task<IActionResult> MyDrive()
        {
            var requestor = await client.Get<InfoEmployee>($"{ApiUrl.EmployeeInfoUrl}/getByUserLogin");
            ViewBag.PageUniqueName = _uniqueName;
            ViewBag.NameEmployee = requestor.EmployeeName;
            ViewBag.EmployeeId = requestor.Id;
            return View();
        }
        
    }
}