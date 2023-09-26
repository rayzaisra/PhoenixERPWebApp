using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.ProjectManagement.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Helpers;
using Phoenix.Shared.Core.Parameters;
using System.Collections.Generic;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.ProjectManagement.Controllers
{
    [Area(PhoenixModule.PM)]
    [Route(PhoenixModule.PM_ROUTE_API)]
    [JwtAuthentication]
    [AllowAnonymousRoleAttribute]
    [ApiExplorerSettings(GroupName = PhoenixModule.PM)]

    public class TypeOfExpenseController : Controller
    {

        private ITypeOfExpenseServices _service;

        public TypeOfExpenseController(ITypeOfExpenseServices service)
        {
            _service = service;
        }

        [HttpGet]
        public GeneralResponseList<TypeOfExpenseDto> List(TypeOfExpenseDto data)
        {
            return _service.ListAll(data);
        }
        
    }
}
