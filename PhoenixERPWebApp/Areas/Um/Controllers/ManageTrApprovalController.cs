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

namespace PhoenixERPWebApp.Areas.Um.Controllers
{
    [Area(nameof(Um))]
    public class ManageTrApprovalController : Controller
    {
        private readonly ApiClientFactory client;

        public ManageTrApprovalController(ApiClientFactory client)
        {
            this.client = client;
        }

        public async Task<IActionResult> ReadHistoryApproval([DataSourceRequest] DataSourceRequest request, string id)
        {
            var model = await client.Get<List<TrUserApproval>>($"{ApiUrl.ManageTransApproval}/GetListApprovalByRefId/{id}") ?? new List<TrUserApproval>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }

}