using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.ApiExtension.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Phoenix.ApiExtension.Attributes
{
    public class ModelStateHandlerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string messages = string.Join("\r\n", context.ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                throw new InvalidModelStateException(messages)
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Status = "Bad Request",
                    ModelState = context.ModelState
                };
            }

            base.OnActionExecuting(context);
        }
    }
}
