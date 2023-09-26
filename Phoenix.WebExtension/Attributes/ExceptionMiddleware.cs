using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Phoenix.WebExtension.Exceptions;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.WebExtension.Attributes
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostingEnvironment hostingEnvironment;
        //private readonly ExceptionResponse exceptionResponse;

        public ExceptionMiddleware(RequestDelegate next, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.hostingEnvironment = hostingEnvironment;
            //this.exceptionResponse = exceptionResponse;
            logger = loggerFactory?.CreateLogger<ExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context, ExceptionResponse exceptionResponse)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();

                await exceptionResponse.ThrowException(ex);
                return;
            }
        }

        //private async Task ThrowError(Exception exception, HttpContext context)
        //{
        //    string source = "Web Exception";
        //    int statusCode = StatusCodes.Status400BadRequest;
        //    string description = exception.Messages();
        //    string status = "Internal Server Error";

        //    if (exception is ApiException)
        //    {
        //        source = "API Exception";
        //        status = (exception as ApiException).Status;
        //        statusCode = (int)(exception as ApiException).StatusCode;

        //    }
        //    else if (exception is HttpRequestException)
        //    {
        //        status = "Bad Request";
        //        statusCode = StatusCodes.Status400BadRequest;
        //    }
        //    else if (exception is UnauthorizedAccessException)
        //    {
        //        status = "Unauthorized";
        //        statusCode = StatusCodes.Status401Unauthorized;
        //    }
        //    else if (exception is ForbiddenAccessException)
        //    {
        //        status = "Forbidden";
        //        statusCode = StatusCodes.Status403Forbidden;
        //    }
        //    else
        //    {
        //        statusCode = StatusCodes.Status500InternalServerError;
        //    }

        //    var errorMessage = exception.Messages();

        //    var m = new
        //    {
        //        errorMessage = errorMessage,
        //        status = status,
        //        statusCode = statusCode,
        //        stackTrace = exception.StackTrace
        //    };

        //    context.Response.StatusCode = statusCode;

        //    if (!IsAjaxRequest(context.Request))
        //    {
        //        var path = $"{hostingEnvironment.WebRootPath}\\error.html";
        //        var text = System.IO.File.ReadAllText(path);

        //        text = text
        //            .Replace("#status_code#", statusCode.ToString())
        //            .Replace("#title#", status)
        //            .Replace("#exception_source#", source)
        //            .Replace("#description#", errorMessage);

        //        context.Response.StatusCode = statusCode;
        //        context.Response.ContentType = "text/html";
        //        await context.Response.WriteAsync(text);
        //    }
        //    else
        //    {
        //        context.Response.StatusCode = statusCode;
        //        context.Response.ContentType = "application/json";
        //        await context.Response.WriteAsync(JsonConvert.SerializeObject(m));
        //    }
        //}

        //private bool IsAjaxRequest(HttpRequest request)
        //{
        //    if (request == null)
        //        return false;

        //    if (request.Headers != null)
        //        return request.Headers["X-Requested-With"] == "XMLHttpRequest";

        //    return false;
        //}
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
