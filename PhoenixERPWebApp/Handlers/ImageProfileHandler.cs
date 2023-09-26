using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Options;
using CodeMarvel.Infrastructure.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Handlers
{
    public class ImageProfileHandler
    {
        private readonly RequestDelegate _next;
        Logger appLogger = LogManager.GetCurrentClassLogger();
        private AppSettingsOption appSetting;

        public ImageProfileHandler(RequestDelegate next, IOptions<AppSettingsOption> AppSetting)
        {
            _next = next;
            appSetting = AppSetting.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var userSession = SessionManager.UserSession;
            if (userSession != null)
                using (var _client = new HttpClient())
                {
                    try
                    {
                        _client.BaseAddress = new Uri(appSetting.ApiServerUrl);
                        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userSession.Token);
                        var recordID = context.Request.Query["id"].ToString();
                        var requestUri = string.Format("core/User/{0}/imageprofile?thumbnail=true", recordID);
                        byte[] task = _client.GetByteArrayAsync(requestUri).Result;
                        if (task != null)
                        {
                            context.Response.ContentType = "image/jpeg";

                            await context.Response.Body.WriteAsync(task);
                            await context.Response.Body.FlushAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        
                    }
                }
        }
    }

    public static class ImageProfileHandlerMiddleware
    {
        public static IApplicationBuilder UseImageProfileHandlerMiddleware
                                     (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageProfileHandler>();
        }
    }

}
