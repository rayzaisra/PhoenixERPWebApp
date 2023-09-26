using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models.Finance.Transaction;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Finance;
using Phoenix.Data.Services.Finance.Transaction;
using PhoenixWebApi.Base;

namespace PhoenixWebApi.Controllers.Finance
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [Route("api/finance/[controller]")]
    public class BudgetManagementController : ApiBaseController
    {
        readonly IBudgetManagementService _service;
        public BudgetManagementController(IBudgetManagementService service)
        {
            this._service = service;
        }

        [HttpPost]
        [Route("uploaddata")]
        [ActionRole(ActionMethod.Add)]
        public async Task<IActionResult> PostUploadData([FromForm]IFormCollection collection)
        {
            //if (collection.Count == 0)
            //{
            //    return Ok(new ApiResponse(null, false, "bad request"));
            //}
            //var model = JsonConvert.DeserializeObject<TrMediaPlanTV>(collection["MediaPlanTV"].ToString());
            var file = collection.Files.GetFile("files");
            var result = await _service.ReadExcel(file);


            return Ok(new ApiResponse(true, Messages.SuccessAdd));
        }
    }
}