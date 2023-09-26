using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.Exceptions
{
    public class MvcException
    {
        public void HandleError(HttpContext httpContext, ControllerBase errorController)
        {
            var currentController = string.Empty;
            var currentAction = string.Empty;


        }

    }
}
