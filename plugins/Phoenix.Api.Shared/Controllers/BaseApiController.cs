using Microsoft.AspNetCore.Mvc;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Api.Shared.Controllers
{
    public partial class BaseApiController<D,T,PK> : Controller where T : BaseEntity<PK> where D : BaseEntity<PK>
    {
        private IBaseService<D, T, PK> _service;

        public BaseApiController(IBaseService<D, T, PK> service)
        {
            _service = service;
        }

    }
}
