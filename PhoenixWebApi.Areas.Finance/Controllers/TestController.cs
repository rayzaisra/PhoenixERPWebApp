using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PhoenixERP;
using Microsoft.AspNetCore.Mvc;
using PhoenixWebApi.Base;

namespace PhoenixWebApi.Areas.Finance.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Finance")]
    [ApiController]
    public class TestController : ApiBaseController
    {
       
    }
}
