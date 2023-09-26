using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.ApiExtension.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Phoenix.ApiExtension.Attributes
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                return;

            var error = new ExceptionResponse
            {
                Status = "System Error",
                StatusCode = HttpStatusCode.InternalServerError,
                Description = context.Exception.Messages(),
                Message = context.Exception.Message,
                Success = false
            };

            if (context.Exception is InvalidModelStateException)
            {
                error.Status = (context.Exception as InvalidModelStateException).Status;
                error.StatusCode = (context.Exception as InvalidModelStateException).StatusCode;
            }
            else if (context.Exception is HttpRequestException)
            {
                error.Status = "Bad Request";
                error.StatusCode = HttpStatusCode.BadRequest;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                error.Status = "Unauthorized";
                error.StatusCode = HttpStatusCode.Unauthorized;
            }
            else if (context.Exception is ForbiddenAccessException)
            {
                error.Status = "Forbidden";
                error.StatusCode = HttpStatusCode.Forbidden;
            }
            else
            {
                error.Status = "Internal System Error";
                error.StatusCode = HttpStatusCode.InternalServerError;
            }

            // always return a JSON result
            context.Result = new JsonResult(error)
            {
                StatusCode = (int)error.StatusCode,
                ContentType = "application/json",
                Value = error
            };

            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}
