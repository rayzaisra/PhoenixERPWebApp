using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;
using System.Collections.Generic;
using Phoenix.Data;
using Phoenix.Data.Models;
using System.Threading.Tasks;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Route("api/general/[controller]")]
    public class MenuController : Controller
    {
        IManageMenuService menu;
        IManageUserAccessService user;

        public MenuController(IManageMenuService menu, IManageUserAccessService user)
        {
            this.menu = menu;
            this.user = user;
        }
        [AllowAnonymousRoleAttribute]
        public async Task<IActionResult> Get()
        {
            var menus = await menu.Get();
            return Ok(new ApiResponse(menus));
        }
        [AllowAnonymousRoleAttribute]
        [HttpGet("auth/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await user.GetById(id);
            return Ok(new ApiResponse(data));
        }

        [HttpGet("raw")]
        public async Task<IActionResult> GetRaw()
        {
            var menus = await menu.GetRaw();
            return Ok(new ApiResponse(menus));
        }
    }
}