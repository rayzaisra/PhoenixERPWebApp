using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Helpers;
using System.Collections.Generic;
using Phoenix.Data.Attributes;

namespace Phoenix.Api.Areas.General.Controllers
{
    [Area(PhoenixModule.GENERAL)]
    [Route(PhoenixModule.GENERAL_ROUTE_API)]
    [AllowAnonymousRoleAttribute]
    [JwtAuthentication]
    [ApiExplorerSettings(GroupName = PhoenixModule.GENERAL)]
    public class NotificationController : Controller
    {
        private INotificationServices _service;

        public NotificationController(INotificationServices service)
        {
            _service = service;
        }
        [HttpGet]
        public GeneralResponseList<NotificationDto> List(NotificationFilter parameter)
        {
            return _service.ListAll(parameter);
        }
        [HttpGet("{id}")]
        public GeneralResponse<NotificationDto> Get(string id)
        {
            return _service.GetNotification(id);
        }
        [HttpPost]
        public BaseGeneralResponse Seen(List<string> id)
        {
            return _service.Seen(id);
        }
        [HttpPost]
        public BaseGeneralResponse Unseen(List<string> id)
        {
            return _service.Unseen(id);
        }
        

    }
}