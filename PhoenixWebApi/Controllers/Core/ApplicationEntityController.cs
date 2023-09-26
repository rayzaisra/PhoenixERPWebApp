using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;
using BusinessLogic.Core;
using BusinessLogic.Services;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP.General;

namespace PhoenixWebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class ApplicationEntityController : ApiBaseController
    {
        [HttpPost]
        [Route("KendoGrid")]
        public async Task<IActionResult> KendoGrid(CommonTool.KendoUI.Grid.DataRequest dtRequest)
        {
            var Service = new ApplicationEntity(AppUserData);
            var listResult = await Task.Run<CommonTool.KendoUI.Grid.DataResponse>(() =>
            {
                return Service.GetListKendoUIGrid(dtRequest);
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }

        [HttpGet]
        [Route("registerEntity")]
        public async Task<IActionResult> RegisterAllEntity()
        {
            
            var service = new ApplicationEntity(AppUserData);
            var detailResult = await Task.Run<ApiResponse>(() =>
            {
                var result = new ApiResponse();
                
                try
                {
                    result.Data = service.RegisterAllEntity();
                    result.Status.Success = true;
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });
            if (detailResult == null) return new EmptyResult();
            else return Ok(detailResult);
        }

    }
}