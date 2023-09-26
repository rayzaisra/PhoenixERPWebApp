using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/core/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Core")]
    public class MenuGroupController : ApiBaseController
    {
        [HttpGet]
        [Route("lookup")]
        public async Task<IActionResult> Lookup([FromQuery(Name = "length")] int length = 100,
          [FromQuery(Name = "filters")] String filters = null, [FromQuery(Name = "parameters")] String parameters = null)
        {
            var Service = new ApplicationMenuGroup(AppUserData);
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