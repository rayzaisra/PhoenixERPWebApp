using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Attributes;
using PhoenixERPWebApp.Models.Finance.Transaction.StatementOfAccountPayable;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    //[MenuArea("590541e3-a932-4d29-b972-ddccb8efc1d7")]
    public class StatementOfAccountPayableController : Controller
    {
        public IActionResult Index()
        {
            var model = new StatementOfAccountPayableIndex();
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            //var model = await client.Get<List<AccountPayable>>(url) ?? new List<AccountPayable>();
            var model = new List<StatementOfAccountPayableList>();
            model.Add(new StatementOfAccountPayableList()
            {
                Id = "001",
                Vendor = "Sugiharto",
                Brand = "Brand1",
                EvidenNo = "EVI001",
                Date = DateTime.Today,
                JobId = "JOB001",
                Division = "IT",
                Support = "Digital",
                Description = "Desc",
                Debet = 0,
                Credit = 2000000,
                Balance = 2000000
            });
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}