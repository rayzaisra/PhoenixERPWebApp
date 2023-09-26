using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Core;
using BusinessLogic.Services;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.PhoenixERP.General;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/core/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class ApplicationModuleController : ApiBaseController
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register()
        {
            var service = new ApplicationModule(AppUserData);
            var saveResult = await Task.Run<ApiResponse>(() =>
            {

                bool isNew = false;
                var result = new ApiResponse();
                try
                {
                    result.Status.Success = service.RegisterApplicationModule();
                    if (result.Status.Success)
                    {
                        if (isNew)
                        {
                            result.Status.Message = "The data has been added.";
                        }
                        else
                        {
                            result.Status.Message = "The data has been updated.";
                        }
                    }
                    else
                    {
                        result.Status.Message = "Record is not saved !";
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Message = ex.Message;
                }
                return result;
            });

            if (saveResult == null) return new EmptyResult();
            else return Ok(saveResult);
        }

        [HttpGet]
        [Route("lookup")]
        public async Task<IActionResult> Lookup([FromQuery(Name = "length")] int length = 100,
            [FromQuery(Name = "filters")] String filters = null, [FromQuery(Name = "parameters")] String parameters = null)
        {
            var Service = new ApplicationModule(AppUserData);
            var listResult = await Task.Run<dynamic>(() =>
            {
                dynamic result = null;
                result = Service.Lookup(Length: length, OptionalFilters: filters, OptionalParameters: parameters);
                return result;
            });
            if (listResult == null) return new EmptyResult();
            else return Ok(listResult);
        }


    }
}